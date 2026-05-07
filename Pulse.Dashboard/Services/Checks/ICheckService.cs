using Pulse.Contracts.Checks;

namespace Pulse.Dashboard.Services.Checks;

public interface ICheckService
{
    Task<IReadOnlyList<CheckDto>> GetChecksAsync(CancellationToken ct);

    Task CreateAsync(CreateCheckRequest request, CancellationToken ct);

    Task SetEnabledAsync(string id, bool enabled, CancellationToken ct);

    Task DeleteAsync(string id, CancellationToken ct);
}
