using Pulse.Contracts.Agents;
using Pulse.Contracts.Common;

namespace Pulse.Web.Services.Agents;

public sealed class AgentService(HttpClient httpClient, ILogger<AgentService> logger) : IAgentService
{
    public async Task<IReadOnlyList<AgentDto>> GetAgentsAsync(CancellationToken cancellationToken = default)
    {
#pragma warning disable S2139 // Exceptions should be either logged or rethrown but not both
        try
        {
            var agents = await httpClient.GetFromJsonAsync<List<AgentDto>>(ApiRoutes.Agents.GetList, cancellationToken);
            return agents?
                .OrderByDescending(x => x.LastSeenUtc)
                .ToArray() ?? [];
        }
        catch (Exception ex) when (ex is HttpRequestException or JsonException or NotSupportedException)
        {
            logger.LogError(ex, "Failed to fetch agents from {Endpoint}", ApiRoutes.Agents.GetList);

            throw;
        }
#pragma warning restore S2139 // Exceptions should be either logged or rethrown but not both
    }
}
