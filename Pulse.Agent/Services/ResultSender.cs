using System.Threading.Channels;

using Pulse.Agent.Contracts;
using Pulse.Agent.Models;
using Pulse.Contracts.Common;
using Pulse.Contracts.Results;

namespace Pulse.Agent.Services;

public class ResultSender : IResultSender, IAsyncDisposable
{
    private readonly Channel<CheckResultDto> _channel;
    private readonly HttpClient _http;
    private readonly ILogger<ResultSender> _logger;

    private readonly int _batchSize = 50;
    private readonly TimeSpan _flushInterval = TimeSpan.FromSeconds(5);

    private readonly CancellationTokenSource _cts;
    private readonly Task _processingTask;
    private AgentIdentity _identity;

    public ResultSender(HttpClient http, ILogger<ResultSender> logger)
    {
        _http = http;
        _logger = logger;

        _channel = Channel.CreateUnbounded<CheckResultDto>();

        _cts = new CancellationTokenSource();
        _processingTask = Task.Run(ProcessLoop);
    }

    public async Task SendAsync(CheckResultDto result, AgentIdentity identity, CancellationToken ct)
    {
        _identity = identity;
        await _channel.Writer.WriteAsync(result, ct);
    }

    private async Task ProcessLoop()
    {
        var buffer = new List<CheckResultDto>(_batchSize);

        using var timer = new PeriodicTimer(_flushInterval);

        try
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                while (_channel.Reader.TryRead(out var item))
                {
                    buffer.Add(item);

                    if (buffer.Count >= _batchSize)
                    {
                        await Flush(buffer);
                        buffer.Clear();
                    }
                }

                await timer.WaitForNextTickAsync(_cts.Token);

                if (buffer.Count > 0)
                {
                    await Flush(buffer);
                    buffer.Clear();
                }
            }
        }
        catch (OperationCanceledException)
        {
            // graceful shutdown
        }
    }

    private async Task Flush(List<CheckResultDto> buffer)
    {
        try
        {
            var request = new SubmitResultsRequest
            {
                Results = buffer.ToList()
            };

            if (_identity == null)
            {
                _logger.LogWarning("Agent identity is not set. Cannot submit results.");
                return;
            }
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _identity.ApiKey);

            var response = await _http.PostAsJsonAsync(ApiRoutes.Results.Submit, request, _cts.Token);

            response.EnsureSuccessStatusCode();
        }
        catch (OperationCanceledException) when (_cts.IsCancellationRequested)
        {
            // shutdown
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send results batch");

            if (_cts.IsCancellationRequested)
            {
                return;
            }

            foreach (var item in buffer)
            {
                _channel.Writer.TryWrite(item);
            }

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(2), _cts.Token);
            }
            catch (OperationCanceledException)
            {
                // shutdown
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        _channel.Writer.TryComplete();
        await _cts.CancelAsync();

        // Wait for background task with timeout to avoid hanging
        try
        {
            await _processingTask.WaitAsync(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
        }
        catch (TimeoutException ex)
        {
            _logger.LogWarning(ex, "Background processing task did not complete within timeout");
        }

        _cts.Dispose();
        GC.SuppressFinalize(this);
    }
}
