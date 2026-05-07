using Pulse.Contracts.Checks;
using Pulse.Server.Agents.Repositories;
using Pulse.Server.Checks.Repositories;
using Pulse.Server.Common;
using Pulse.Server.Common.Extensions;

namespace Pulse.Server.Checks.GetChecks;

public class GetAgentCheckQueryHandler : IEndpointHandler
{
    private readonly ICheckRepository _checks;
    private readonly IAgentRepository _agents;

    public GetAgentCheckQueryHandler(ICheckRepository checks, IAgentRepository agents)
    {
        _checks = checks;
        _agents = agents;
    }

    public async Task<List<CheckDefinitionDto>> HandleAsync(HttpContext context, CancellationToken ct)
    {
        string apiKey = context.GetApiKey();

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new UnauthorizedAccessException();
        }

        var agent = await _agents.GetByApiKeyAsync(apiKey, ct) ?? throw new UnauthorizedAccessException();

        // Update heartbeat
        agent.LastSeenUtc = DateTime.UtcNow;
        await _agents.SaveChangesAsync(ct);

        var checks = await _checks.GetEnabledAsync(ct);

        return checks.ConvertAll(x => new CheckDefinitionDto
        {
            Id = x.Id,
            Type = x.Type,
            Name = x.Name,
            Target = x.Target,
            IntervalSeconds = x.IntervalSeconds,
            TimeoutMs = x.TimeoutMs
        });
    }
}
