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

            var response = await _http.GetAsync(check.Target, cts.Token);

            sw.Stop();

            if ((int)response.StatusCode >= 200 && (int)response.StatusCode < 300)
            {
                return CheckResultFactory.Success(check.Id, sw.ElapsedMilliseconds);
            }

            return CheckResultFactory.Failure(
                check.Id,
                sw.ElapsedMilliseconds,
                $"HTTP {(int)response.StatusCode}"
            );
        }
        catch (Exception ex)
        {
            sw.Stop();

            return CheckResultFactory.Failure(
                check.Id,
                sw.ElapsedMilliseconds,
                ex.Message
            );
        }
    }
}
