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
}
