using Pulse.Contracts.Checks;
using Pulse.Contracts.Results;

namespace Pulse.Agent.Contracts;

public interface ICheckExecutor
{
    Task<CheckResultDto> ExecuteAsync(CheckDefinitionDto check, CancellationToken ct);
}
