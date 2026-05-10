using Pulse.Contracts.CheckRuns;
using Pulse.Server.CheckRuns.Repositories;
using Pulse.Server.Common;

namespace Pulse.Server.CheckRuns.Radar;

public class GetRadarQueryHandler : IEndpointHandler
{
    private readonly ICheckResultRepository _repository;

    public GetRadarQueryHandler(ICheckResultRepository repository)
    {
        _repository = repository;
    }

    internal async Task<IReadOnlyList<RadarCheckDto>> HandleAsync(CancellationToken ct)
    {
        return await _repository.GetRadarAsync(ct);
    }
}
