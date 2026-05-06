using Pulse.Contracts.Results;

namespace Pulse.Agent.Contracts;

public interface IResultSender
{
    Task SendAsync(CheckResultDto result, CancellationToken ct);
}
