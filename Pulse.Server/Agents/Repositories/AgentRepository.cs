using Microsoft.EntityFrameworkCore;

using Pulse.Server.Agents.Entities;
using Pulse.Server.Data;

namespace Pulse.Server.Agents.Repositories;

public class AgentRepository : IAgentRepository
{
    private readonly ApplicationDbContext _db;

    public AgentRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public Task<Agent> GetByIdAsync(string id, CancellationToken ct)
    {
        return _db.Agents.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public Task<Agent> GetByApiKeyAsync(string apiKey, CancellationToken ct)
    {
        return _db.Agents.FirstOrDefaultAsync(x => x.ApiKey == apiKey, ct);
    }

    public Task<Agent> GetByMachineIdAsync(string machineId, CancellationToken ct)
    {
        return _db.Agents.FirstOrDefaultAsync(x => x.MachineId == machineId, ct);
    }

    public async Task AddAsync(Agent agent, CancellationToken ct)
    {
        await _db.Agents.AddAsync(agent, ct);
    }

    public Task SaveChangesAsync(CancellationToken ct)
    {
        return _db.SaveChangesAsync(ct);
    }
}
