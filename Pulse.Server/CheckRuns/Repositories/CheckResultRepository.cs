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
        var result = await _db.Results.AsNoTracking()
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

        var agents = await _db.Agents
            .AsNoTracking()
            .ToDictionaryAsync(x => x.Id, ct);

        result.ForEach(x =>
        {
            if (agents.TryGetValue(x.AgentId, out var agent))
            {
                x.AgentName = agent.Hostname;
            }
        });

        return result;
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
        var checks = await _db.Checks
       .AsNoTracking()
       .Where(x => x.Enabled)
       .OrderBy(x => x.Name)
       .ToListAsync(ct);

        var agents = await _db.Agents
            .AsNoTracking()
            .ToDictionaryAsync(
                x => x.Id,
                ct);

        var results = await _db.Results
            .AsNoTracking()
            .OrderByDescending(x => x.TimestampUtc)
            .Select(x => new
            {
                x.CheckId,
                x.AgentId,

                x.IsSuccess,
                x.ResponseTimeMs,
                x.Error,
                x.TimestampUtc
            })
            .ToListAsync(ct);

        var groupedResults = results
            .GroupBy(x => new
            {
                x.CheckId,
                x.AgentId
            })
            .ToDictionary(
                x => x.Key,
                x => x.Take(5).ToList());

        var radar = new List<RadarCheckDto>();

        foreach (var check in checks)
        {
            var checkDto = new RadarCheckDto
            {
                CheckId = check.Id,
                CheckName = check.Name
            };

            var agentGroups = groupedResults
                .Where(x => string.Equals(x.Key.CheckId, check.Id, StringComparison.Ordinal));

            foreach (var agentGroup in agentGroups)
            {
                if (!agents.TryGetValue(
                        agentGroup.Key.AgentId,
                        out var agent))
                {
                    continue;
                }

                var agentDto = new RadarAgentDto
                {
                    AgentId = agent.Id,
                    AgentName = agent.Hostname,
                    LastHeartbeatUtc = agent.LastSeenUtc,

                    Results = agentGroup.Value
                        .Select(x => new RadarResultDto
                        {
                            IsSuccess = x.IsSuccess,
                            ResponseTimeMs = x.ResponseTimeMs,
                            Error = x.Error,
                            TimestampUtc = x.TimestampUtc
                        })
                        .OrderByDescending(x => x.TimestampUtc)
                        .ToList()
                };

                checkDto.Agents.Add(agentDto);
            }

            checkDto.Agents = checkDto.Agents
                .OrderBy(x => x.AgentName)
                .ToList();

            radar.Add(checkDto);
        }

        return radar;
    }
}
