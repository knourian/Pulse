using Pulse.Contracts.Checks;
using Pulse.Server.Checks.Entities;

namespace Pulse.Server.Checks.Repositories;

public interface ICheckRepository
{
    Task AddAsync(Check check, CancellationToken ct);
    Task<List<CheckDto>> GetAllChecks(CancellationToken ct);
    Task<Check> GetByIdAsync(string id, CancellationToken ct);
    Task<List<Check>> GetEnabledAsync(CancellationToken ct);
    Task Remove(Check check);
    Task SaveChangesAsync(CancellationToken ct);
}
