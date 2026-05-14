using Pulse.Agent.Contracts;
using Pulse.Agent.Helpers;
using Pulse.Contracts.CheckRuns;
using Pulse.Contracts.Checks;

namespace Pulse.Agent.Engines;

public class HttpCheckExecutor : ICheckExecutor
{
    private readonly HttpClient _http;

    public HttpCheckExecutor(HttpClient http)
    {
        _http = http;
    }

    public async Task<CheckResultDto> ExecuteAsync(CheckDefinitionDto check, CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(check.TimeoutMs);

            var statusCode = await GetStatusCodeAsync(check.Target, cts.Token);

            sw.Stop();

            if ((int)statusCode >= 200 && (int)statusCode < 300)
            {
                return CheckResultFactory.Success(check.Id, sw.ElapsedMilliseconds);
            }

            return CheckResultFactory.Failure(
                check.Id,
                sw.ElapsedMilliseconds,
                $"HTTP {(int)statusCode}"
            );
        }
        catch (Exception ex)
        {
            sw.Stop();
            return CheckResultFactory.Failure(check.Id, sw.ElapsedMilliseconds, ex.Message);
        }
    }

    private async Task<System.Net.HttpStatusCode> GetStatusCodeAsync(string target, CancellationToken ct)
    {
        var request = new HttpRequestMessage(HttpMethod.Head, target);
        var response = await _http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);

        if (response.StatusCode == System.Net.HttpStatusCode.MethodNotAllowed)
        {
            var getRequest = new HttpRequestMessage(HttpMethod.Get, target);
            response = await _http.SendAsync(getRequest, HttpCompletionOption.ResponseHeadersRead, ct);
        }

        return response.StatusCode;
    }
}