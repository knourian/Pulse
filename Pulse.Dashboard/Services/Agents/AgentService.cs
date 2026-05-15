using Pulse.Contracts.Agents;
using Pulse.Contracts.Common;

namespace Pulse.Dashboard.Services.Agents;

public sealed class AgentService(HttpClient httpClient, ILogger<AgentService> logger) : IAgentService
{
    public async Task<IReadOnlyList<AgentDto>> GetAgentsAsync(CancellationToken cancellationToken = default)
    {
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

            throw new InvalidOperationException("Failed to fetch agents", ex);
        }
    }

    public async Task SetAgentHostname(string id, string hostName, CancellationToken ct)
    {
        try
        {
            var request = new EditAgentRequest
            {
                Hostname = hostName
            };

            var response = await httpClient.PutAsJsonAsync(ApiRoutes.Agents.EditHostname(id), request, ct);

            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update agent hostname agent: {AgentId}", id);

            throw new InvalidOperationException("Failed to update agent hostname agent", ex);
        }
    }
}
