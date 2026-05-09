using Pulse.Contracts.CheckRuns;
using Pulse.Contracts.Common;

namespace Pulse.Dashboard.Services.CheckResults;

public class CheckResultService : ICheckResultService
{
    private readonly HttpClient _http;
    private readonly ILogger<CheckResultService> _logger;

    public CheckResultService(HttpClient http, ILogger<CheckResultService> logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task<IReadOnlyList<CheckResultDto>> GetResultsAsync(string checkId, CancellationToken ct)
    {
        try
        {
            var response = await _http.GetAsync(ApiRoutes.Results.Get(checkId), ct);

            response.EnsureSuccessStatusCode();

            var results = await response.Content.ReadFromJsonAsync<List<CheckResultDto>>(cancellationToken: ct);

            return results ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load results for check: {CheckId}", checkId);

            throw new InvalidOperationException("Failed to load results for check.", ex);
        }
    }
}
