using Pulse.Agent.Models;
using Pulse.Contracts.CheckRuns;

namespace Pulse.Agent.Contracts;

public interface IResultSender
{
    Task SendAsync(CheckResultDto result, AgentIdentity identity, CancellationToken ct);
}
