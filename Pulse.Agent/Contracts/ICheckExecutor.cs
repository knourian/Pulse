using Pulse.Contracts.CheckRuns;
using Pulse.Contracts.Checks;

namespace Pulse.Agent.Contracts;

public interface ICheckExecutor
{
    Task<CheckResultDto> ExecuteAsync(CheckDefinitionDto check, CancellationToken ct);
}
