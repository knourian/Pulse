using Pulse.Contracts.Agents;

namespace Pulse.Dashboard.Services.Agents;

public interface IAgentService
{
    Task<IReadOnlyList<AgentDto>> GetAgentsAsync(CancellationToken cancellationToken = default);
}
