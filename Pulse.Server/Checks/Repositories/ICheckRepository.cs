using Pulse.Server.Checks.Entities;

namespace Pulse.Server.Checks.Repositories;

public interface ICheckRepository
{
    Task<List<CheckDto>> GetAllChecks(CancellationToken ct);
    Task<List<Check>> GetEnabledAsync(CancellationToken ct);
}
