using Microsoft.Extensions.Options;

using Pulse.Agent.Models;
using Pulse.Agent.Services;

namespace Pulse.Agent;

public class Worker : BackgroundService
{
    private readonly AgentRegistrationService _registrationService;
    private readonly AgentIdentityStore _identityStore;
    private readonly ILogger<Worker> _logger;
    private readonly AppSetting _settings;

    public Worker(IOptions<AppSetting> settings,
                  AgentRegistrationService registrationService,
                  AgentIdentityStore identityStore,
                  ILogger<Worker> logger)
    {
        _settings = settings.Value;
        _registrationService = registrationService;
        _identityStore = identityStore;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var identity = await _identityStore.LoadAsync(stoppingToken);

        if (identity == null)
        {
            identity = await RegisterWithRetryAsync(stoppingToken);

            await _identityStore.SaveAsync(identity, stoppingToken);
            _logger.LogInformation("Agent registered with id: {AgentId}", identity.AgentId);
        }
        else
        {
            _logger.LogInformation("Loaded existing agent identity: {AgentId}", identity.AgentId);
        }


        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(_settings.Agent.HeartbeatSeconds));

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.LogInformation("Worker heartbeat at: {Time}", DateTimeOffset.UtcNow);
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // Graceful shutdown.
        }
    }


    private async Task<AgentIdentity> RegisterWithRetryAsync(CancellationToken ct)
    {
        Exception lastException = null;

        for (var attempt = 1; attempt <= _settings.Agent.RegistrationMaxAttempts; attempt++)
        {
            try
            {
                var response = await _registrationService.RegisterAsync(ct);
                return new AgentIdentity
                {
                    AgentId = response.AgentId,
                    ApiKey = response.ApiKey
                };
            }
            catch (Exception ex) when (!ct.IsCancellationRequested)
            {
                lastException = ex;
                _logger.LogWarning(ex, "Agent registration attempt {Attempt}/{MaxAttempts} failed.", attempt, _settings.Agent.RegistrationMaxAttempts);

                if (attempt == _settings.Agent.RegistrationMaxAttempts)
                {
                    break;
                }

                var delay = TimeSpan.FromSeconds(_settings.Agent.RegistrationRetrySeconds * attempt);
                await Task.Delay(delay, ct);
            }
        }

        throw new InvalidOperationException("Agent registration failed after all retry attempts.", lastException);
    }
}
