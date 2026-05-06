using Microsoft.EntityFrameworkCore;

using Pulse.Server.Checks.Entities;
using Pulse.Server.Data;

namespace Pulse.Server.Checks.Repositories;

public class CheckRepository : ICheckRepository
{
    private readonly ApplicationDbContext _db;

    public CheckRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<Check>> GetEnabledAsync(CancellationToken ct)
    {
        return await _db.Checks
            .Where(x => x.Enabled)
            .AsNoTracking()
            .ToListAsync(ct);
    }
}
