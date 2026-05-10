using Microsoft.EntityFrameworkCore;

using Pulse.Contracts.CheckRuns;
using Pulse.Server.CheckRuns.Entities;
using Pulse.Server.Data;

namespace Pulse.Server.CheckRuns.Repositories;

public class CheckResultRepository : ICheckResultRepository
{
    private readonly ApplicationDbContext _db;

    public CheckResultRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<CheckResultDto>> GetRecentByCheckIdAsync(string checkId, int limit, CancellationToken ct)
    {
        return await _db.Results.AsNoTracking()
        .Where(x => x.CheckId == checkId)
        .OrderByDescending(x => x.TimestampUtc)
        .Take(limit)
        .Select(x => new CheckResultDto
        {
            CheckId = x.CheckId,
            AgentId = x.AgentId,
            TimestampUtc = x.TimestampUtc,
            IsSuccess = x.IsSuccess,
            ResponseTimeMs = x.ResponseTimeMs,
            Error = x.Error
        })
        .ToListAsync(ct);
    }

    public async Task AddRangeAsync(List<CheckResult> results, CancellationToken ct)
    {
        await _db.Results.AddRangeAsync(results, ct);
    }

    public Task SaveChangesAsync(CancellationToken ct)
    {
        return _db.SaveChangesAsync(ct);
    }

    public async Task<List<RadarCheckDto>> GetRadarAsync(
        CancellationToken ct)
    {
        var latestPerCheckAndAgent = _db.Results
            .AsNoTracking()
            .GroupBy(x => new
            {
                x.CheckId,
                x.AgentId
            })
            .Select(g => new
            {
                g.Key.CheckId,
                g.Key.AgentId,
                TimestampUtc = g.Max(x => x.TimestampUtc)
            });

        var latestResults = await _db.Results
            .AsNoTracking()
            .Join(
                latestPerCheckAndAgent,
                result => new
                {
                    result.CheckId,
                    result.AgentId,
                    result.TimestampUtc
                },
                latest => new
                {
                    latest.CheckId,
                    latest.AgentId,
                    latest.TimestampUtc
                },
                (result, _) => result)
            .Join(_db.Checks,
                result => result.CheckId,
                check => check.Id,
                (result, check) => new
                {
                    CheckId = check.Id,
                    CheckName = check.Name,

                    result.AgentId,
                    result.IsSuccess,
                    result.ResponseTimeMs,
                    result.Error,
                    result.TimestampUtc
                })
            .Join(
                _db.Agents,
                result => result.AgentId,
                agent => agent.Id,
                (result, agent) => new
                {
                    result.CheckId,
                    result.CheckName,

                    AgentId = agent.Id,
                    AgentName = agent.Hostname,

                    result.IsSuccess,
                    result.ResponseTimeMs,
                    result.Error,
                    result.TimestampUtc
                })
            .ToListAsync(ct);

        return latestResults
            .GroupBy(x => new
            {
                x.CheckId,
                x.CheckName
            })
            .Select(group => new RadarCheckDto
            {
                CheckId = group.Key.CheckId,
                CheckName = group.Key.CheckName,

                Agents = group
                    .Select(x => new RadarAgentResultDto
                    {
                        AgentId = x.AgentId,
                        AgentName = x.AgentName,
                        IsSuccess = x.IsSuccess,
                        ResponseTimeMs = x.ResponseTimeMs,
                        Error = x.Error,
                        TimestampUtc = x.TimestampUtc
                    })
                    .OrderBy(x => x.AgentName)
                    .ToList()
            })
            .OrderBy(x => x.CheckName)
            .ToList();
    }
}
