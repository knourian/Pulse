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

    public async Task AddRangeAsync(List<CheckResult> results, CancellationToken ct)
    {
        await _db.Results.AddRangeAsync(results, ct);
    }

    public Task SaveChangesAsync(CancellationToken ct)
    {
        return _db.SaveChangesAsync(ct);
    }
}
