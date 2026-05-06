using Pulse.Server.Checks.Entities;

namespace Pulse.Server.Checks.Repositories;

public interface ICheckRepository
{
    Task<List<Check>> GetEnabledAsync(CancellationToken ct);
}
