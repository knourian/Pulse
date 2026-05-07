using Pulse.Contracts.Checks;
using Pulse.Server.Checks.Repositories;
using Pulse.Server.Common;

namespace Pulse.Server.Checks.GetChecks;

public class GetChecksQueryHandler : IEndpointHandler
{
    private readonly ICheckRepository _repository;

    public GetChecksQueryHandler(ICheckRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CheckDto>> HandleAsync(CancellationToken ct)
    {
        return await _repository.GetAllChecks(ct);
    }
}
