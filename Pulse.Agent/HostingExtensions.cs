using Microsoft.Extensions.Options;

using Pulse.Agent.Contracts;
using Pulse.Agent.Engines;
using Pulse.Agent.Models;
using Pulse.Agent.Services;

namespace Pulse.Agent;

public static class HostingExtensions
{
    public static HostApplicationBuilder RegisterServices(this HostApplicationBuilder builder)
    {
        builder.Services.AddOptions<AppSetting>().Bind(builder.Configuration)
        .Validate(
            s => Uri.TryCreate(s.PulseServer.BaseUrl, UriKind.Absolute, out _),
            "Invalid 'PulseServer:BaseUrl'.")
        .Validate(
            s => !string.IsNullOrWhiteSpace(s.Agent.IdentityEncryptionKey),
            "Missing 'Agent:IdentityEncryptionKey'.")
        .ValidateOnStart();

        builder.Services.AddSingleton<AgentIdentityStore>();
        builder.Services.AddSingleton<CheckScheduler>();

        builder.Services.AddHttpClient<AgentRegistrationService>(HttpClientConfiguration());
        builder.Services.AddHttpClient<CheckProviderService>(HttpClientConfiguration());

        builder.Services.AddHttpClient<HttpCheckExecutor>();

        builder.Services.AddTransient<TcpCheckExecutor>();
        builder.Services.AddTransient<PingCheckExecutor>();
        builder.Services.AddTransient<ICheckExecutorFactory, CheckExecutorFactory>();

        builder.Services.AddHttpClient("PulseServer", HttpClientConfiguration());

        builder.Services.AddSingleton<IResultSender>(sp =>
        {
            var http = sp.GetRequiredService<IHttpClientFactory>().CreateClient("PulseServer");
            var logger = sp.GetRequiredService<ILogger<ResultSender>>();
            return new ResultSender(http, logger);
        });

        builder.Services.AddHostedService<Worker>();


        return builder;
    }

    private static Action<IServiceProvider, HttpClient> HttpClientConfiguration()
    {
        return (sp, http) =>
        {
            var settings = sp.GetRequiredService<IOptions<AppSetting>>().Value;

            http.BaseAddress = new Uri(settings.PulseServer.BaseUrl, UriKind.Absolute);
            http.Timeout = TimeSpan.FromSeconds(30);
        };
    }
}
