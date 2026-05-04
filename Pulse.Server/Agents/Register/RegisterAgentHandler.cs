using Pulse.Contracts.Agents;
using Pulse.Server.Agents.Entities;
using Pulse.Server.Agents.Repositories;
using Pulse.Server.Common;

namespace Pulse.Server.Agents.Register;

public class RegisterAgentHandler : IEndpointHandler
{
    private readonly IAgentRepository _agents;

    public RegisterAgentHandler(IAgentRepository agents)
    {
        _agents = agents;
    }

    public async Task<RegisterAgentResponse> HandleAsync(
        RegisterAgentRequest request,
        CancellationToken ct)
    {

        var existing = await _agents.GetByMachineIdAsync(request.MachineId, ct);
        if (existing != null)
        {
            // update heartbeat
            existing.LastSeenUtc = DateTime.UtcNow;

            await _agents.SaveChangesAsync(ct);

            return new RegisterAgentResponse
            {
                AgentId = existing.Id,
                ApiKey = existing.ApiKey
            };
        }


        var agent = new Agent
        {
            Id = Guid.NewGuid().ToString("N"),
            Hostname = request.Hostname.Trim().ToLowerInvariant(),
            ApiKey = GenerateApiKey(),
            LastSeenUtc = DateTime.UtcNow
        };

        await _agents.AddAsync(agent, ct);
        await _agents.SaveChangesAsync(ct);

        return new RegisterAgentResponse
        {
            AgentId = agent.Id,
            ApiKey = agent.ApiKey
        };
    }

    private static string GenerateApiKey()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}
