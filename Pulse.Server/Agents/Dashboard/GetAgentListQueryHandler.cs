using Pulse.Contracts.Agents;
using Pulse.Server.Agents.Repositories;
using Pulse.Server.Common;

namespace Pulse.Server.Agents.Dashboard;

public class GetAgentListQueryHandler : IEndpointHandler
{
    private readonly IAgentRepository _agents;

    public GetAgentListQueryHandler(IAgentRepository agents)
    {
        _agents = agents;
    }

    public async Task<List<AgentDto>> HandleAsync(CancellationToken ct)
    {
        var agents = await _agents.GetAllAsync(ct);
        return agents.ToList();
    }
}
