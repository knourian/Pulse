using Pulse.Server.CheckRuns.Entities;

namespace Pulse.Server.CheckRuns.Repositories;

public interface ICheckResultRepository
{
    Task AddRangeAsync(List<CheckResult> results, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
