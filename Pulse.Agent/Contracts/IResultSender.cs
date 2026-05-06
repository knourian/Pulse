using Pulse.Agent.Models;
using Pulse.Contracts.Results;

namespace Pulse.Agent.Contracts;

public interface IResultSender
{
    Task SendAsync(CheckResultDto result, AgentIdentity identity, CancellationToken ct);
}
