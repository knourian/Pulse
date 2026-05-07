using Microsoft.EntityFrameworkCore;

using Pulse.Server.Checks.Entities;
using Pulse.Server.Data;

namespace Pulse.Server.Checks.Repositories;

public class CheckRepository : ICheckRepository
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<CheckRepository> _logger;

    public CheckRepository(ApplicationDbContext db, ILogger<CheckRepository> logger)
    {
        _db = db;
        _logger = logger;
    }
    public async Task<List<CheckDto>> GetAllChecks(CancellationToken ct)
    {
        return await _db.Checks.AsNoTracking().Select(x => new CheckDto
        {
            Id = x.Id,
            Name = x.Name,
            Type = x.Type,
            Target = x.Target,
            IntervalSeconds = x.IntervalSeconds,
            TimeoutMs = x.TimeoutMs,
            Enabled = x.Enabled,
            CreatedUtc = x.CreatedUtc
        }).ToListAsync(ct);
    }
    }

    public async Task<List<Check>> GetEnabledAsync(CancellationToken ct)
    {
        return await _db.Checks
            .Where(x => x.Enabled)
            .AsNoTracking()
            .ToListAsync(ct);
    }
}
