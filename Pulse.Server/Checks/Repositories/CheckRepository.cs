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

    public async Task AddAsync(Check check, CancellationToken ct)
    {
        await _db.Checks.AddAsync(check, ct);
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
    public async Task SaveChangesAsync(CancellationToken ct)
    {
        try
        {
            await _db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            throw new InvalidOperationException("A check with the same type and target already exists.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving changes to the database.");
            throw new InvalidOperationException("An error occurred while saving changes to the database.", ex);
        }
    }
}
