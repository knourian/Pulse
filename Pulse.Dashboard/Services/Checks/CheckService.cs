using Pulse.Contracts.Checks;
using Pulse.Contracts.Common;

namespace Pulse.Dashboard.Services.Checks;

public class CheckService : ICheckService
{
    private readonly HttpClient _http;
    private readonly ILogger<CheckService> _logger;

    public CheckService(HttpClient http, ILogger<CheckService> logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task CreateAsync(CreateCheckRequest request, CancellationToken ct)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(ApiRoutes.Checks.CreateCheck, request, ct);

            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create check.");
            throw new InvalidOperationException("Failed to create check.", ex);
        }
    }

    public async Task DeleteAsync(string id, CancellationToken ct)
    {
        try
        {
            var response = await _http.DeleteAsync(ApiRoutes.Checks.DeleteCheck(id), ct);

            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete check: {CheckId}", id);

            throw new InvalidOperationException("Failed to delete check.", ex);
        }
    }

    public async Task<IReadOnlyList<CheckDto>> GetChecksAsync(CancellationToken ct)
    {
        try
        {
            var response = await _http.GetAsync(ApiRoutes.Checks.GetCheck, ct);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<CheckDto>>(cancellationToken: ct);

            return result ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load checks.");
            throw new InvalidOperationException("Failed to load checks.", ex);
        }
    }

    public async Task SetEnabledAsync(string id, bool enabled, CancellationToken ct)
    {
        try
        {
            var request = new SetCheckEnabledRequest
            {
                Enabled = enabled
            };

            var response = await _http.PutAsJsonAsync(ApiRoutes.Checks.SetCheckEnabled(id), request, ct);

            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update enabled state for check: {CheckId}", id);

            throw new InvalidOperationException("Failed to update enabled state for check.", ex);
        }
    }
}
