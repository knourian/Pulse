using Pulse.Agent.Helpers;
using Pulse.Contracts.Agents;
using Pulse.Contracts.Common;

namespace Pulse.Agent.Services;

public class AgentRegistrationService
{
    private readonly HttpClient _http;

    public AgentRegistrationService(HttpClient http)
    {
        _http = http;
    }

    public async Task<RegisterAgentResponse> RegisterAsync(CancellationToken ct)
    {
        var request = new RegisterAgentRequest
        {
            Hostname = Environment.MachineName,
            MachineId = MachineIdProvider.Get()
        };

        var response = await _http.PostAsJsonAsync(ApiRoutes.Agents.Register, request, ct);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<RegisterAgentResponse>(cancellationToken: ct)
                             ?? throw new InvalidOperationException("Invalid registration response.");

        if (string.IsNullOrWhiteSpace(result.AgentId) || string.IsNullOrWhiteSpace(result.ApiKey))
        {
            throw new InvalidOperationException("Registration response is missing required fields.");
        }

        return result;
    }
}
