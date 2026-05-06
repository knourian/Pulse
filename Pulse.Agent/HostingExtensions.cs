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
        builder.Services.AddHttpClient<AgentRegistrationService>((sp, http) =>
        {
            var settings = sp.GetRequiredService<IOptions<AppSetting>>().Value;

            http.BaseAddress = new Uri(settings.PulseServer.BaseUrl, UriKind.Absolute);
            http.Timeout = TimeSpan.FromSeconds(30);
        });

        builder.Services.AddSingleton<CheckProviderService>();
        builder.Services.AddHttpClient<CheckProviderService>((sp, http) =>
        {
            var settings = sp.GetRequiredService<IOptions<AppSetting>>().Value;

            http.BaseAddress = new Uri(settings.PulseServer.BaseUrl, UriKind.Absolute);
            http.Timeout = TimeSpan.FromSeconds(30);
        });

        builder.Services.AddSingleton<CheckScheduler>();

        builder.Services.AddTransient<HttpCheckExecutor>();
        builder.Services.AddHttpClient<HttpCheckExecutor>();

        builder.Services.AddTransient<TcpCheckExecutor>();
        builder.Services.AddTransient<PingCheckExecutor>();

        builder.Services.AddTransient<ICheckExecutorFactory, CheckExecutorFactory>();

        //builder.Services.AddScoped<IResultSender, ResultSender>();

        builder.Services.AddHostedService<Worker>();


        return builder;
    }
}
