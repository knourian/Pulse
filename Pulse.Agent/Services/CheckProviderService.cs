using Pulse.Agent.Models;
using Pulse.Contracts.Checks;
using Pulse.Contracts.Common;

namespace Pulse.Agent.Services;

public class CheckProviderService
{
    private readonly HttpClient _http;

    public CheckProviderService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<CheckDefinitionDto>> GetChecksAsync(AgentIdentity identity, CancellationToken ct)
    {

        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", identity.ApiKey);

        var response = await _http.GetAsync(ApiRoutes.Checks.Get, ct);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<CheckDefinitionDto>>(cancellationToken: ct)
            ?? new List<CheckDefinitionDto>();
    }
}
