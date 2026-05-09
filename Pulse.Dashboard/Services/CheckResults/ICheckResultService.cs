using Pulse.Contracts.CheckRuns;

namespace Pulse.Dashboard.Services.CheckResults;

public interface ICheckResultService
{
    Task<IReadOnlyList<CheckResultDto>> GetResultsAsync(string checkId, CancellationToken ct);
}
