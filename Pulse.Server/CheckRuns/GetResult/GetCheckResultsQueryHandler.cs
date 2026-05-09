using Pulse.Contracts.CheckRuns;
using Pulse.Server.CheckRuns.Repositories;
using Pulse.Server.Common;

namespace Pulse.Server.CheckRuns.GetResult;

public class GetCheckResultsQueryHandler : IEndpointHandler
{
    private readonly ICheckResultRepository _repository;

    public GetCheckResultsQueryHandler(ICheckResultRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<CheckResultDto>> HandleAsync(
            string checkId,
            CancellationToken ct)
    {
        var results = await _repository.GetRecentByCheckIdAsync(
            checkId,
            100,
            ct);

        return results;
    }
}
