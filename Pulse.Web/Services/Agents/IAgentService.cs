using Pulse.Contracts.Agents;

namespace Pulse.Web.Services.Agents;

public interface IAgentService
{
    Task<IReadOnlyList<AgentDto>> GetAgentsAsync(CancellationToken cancellationToken = default);
}
