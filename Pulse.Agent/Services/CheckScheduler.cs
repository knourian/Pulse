using Pulse.Agent.Contracts;
using Pulse.Agent.Models;
using Pulse.Contracts.Checks;

namespace Pulse.Agent.Services;

public class CheckScheduler
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<CheckScheduler> _logger;

    private readonly Dictionary<string, RunningCheck> _running = new();

    public CheckScheduler(IServiceProvider provider, ILogger<CheckScheduler> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public void Sync(List<CheckDefinitionDto> checks)
    {
        var incomingIds = checks.Select(x => x.Id).ToHashSet();

        var toRemove = _running.Keys
            .Where(id => !incomingIds.Contains(id))
            .ToList();

        foreach (var id in toRemove)
        {
            Stop(id);
        }

        foreach (var check in checks)
        {
            if (_running.TryGetValue(check.Id, out var existing))
            {
                if (IsChanged(existing.Definition, check))
                {
                    _logger.LogInformation("Check changed: {CheckId}", check.Id);
                    Stop(check.Id);
                    Start(check);
                }
            }
            else
            {
                Start(check);
            }
        }
    }

    private void Start(CheckDefinitionDto check)
    {
        var cts = new CancellationTokenSource();

        var task = Task.Run(() => RunCheckLoop(check, cts.Token));

        _running[check.Id] = new RunningCheck
        {
            Definition = check,
            Cts = cts,
            Task = task
        };

        _logger.LogInformation("Started check: {CheckId}", check.Id);
    }

    private void Stop(string checkId)
    {
        if (_running.TryGetValue(checkId, out var running))
        {
            running.Cts.Cancel();

            _running.Remove(checkId);

            _logger.LogInformation("Stopped check: {CheckId}", checkId);
        }
    }

    private static bool IsChanged(CheckDefinitionDto a, CheckDefinitionDto b)
    {
        return a.Type != b.Type
               || !string.Equals(a.Target, b.Target, StringComparison.OrdinalIgnoreCase)
               || a.IntervalSeconds != b.IntervalSeconds
               || a.TimeoutMs != b.TimeoutMs;
    }

    private async Task RunCheckLoop(CheckDefinitionDto check, CancellationToken ct)
    {
        using var scope = _provider.CreateScope();

        var executorFactory = scope.ServiceProvider.GetRequiredService<ICheckExecutorFactory>();
        var resultSender = scope.ServiceProvider.GetRequiredService<IResultSender>();

        var executor = executorFactory.Create(check.Type);

        var interval = TimeSpan.FromSeconds(check.IntervalSeconds);

        while (!ct.IsCancellationRequested)
        {
            try
            {
                var result = await executor.ExecuteAsync(check, ct);

                await resultSender.SendAsync(result, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing check {CheckId}", check.Id);
            }

            try
            {
                await Task.Delay(interval, ct);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }
}