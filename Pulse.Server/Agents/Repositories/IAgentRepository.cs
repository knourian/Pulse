using Pulse.Server.Agents.Entities;

namespace Pulse.Server.Agents.Repositories;

public interface IAgentRepository
{
    Task<Agent> GetByIdAsync(string id, CancellationToken ct);
    Task<Agent> GetByApiKeyAsync(string apiKey, CancellationToken ct);
    Task<Agent> GetByMachineIdAsync(string machineId, CancellationToken ct);
    Task AddAsync(Agent agent, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
