using Pulse.Server.Checks.Entities;

namespace Pulse.Server.Checks.Repositories;

public interface ICheckRepository
{
    Task AddAsync(Check check, CancellationToken ct);
    Task<List<CheckDto>> GetAllChecks(CancellationToken ct);
    Task<List<Check>> GetEnabledAsync(CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
