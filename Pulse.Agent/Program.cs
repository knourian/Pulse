using Microsoft.Extensions.Options;

using Pulse.Agent;
using Pulse.Agent.Models;
using Pulse.Agent.Services;

LoadDotEnv();

var builder = Host.CreateApplicationBuilder(args);


builder.Services
    .AddOptions<AppSetting>()
    .Bind(builder.Configuration)
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

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
await host.RunAsync();



static void LoadDotEnv()
{
    var candidates = new[]
    {
        Path.Combine(AppContext.BaseDirectory, ".env"),
        Path.Combine(Directory.GetCurrentDirectory(), ".env")
    };

    var envPath = candidates.FirstOrDefault(File.Exists);
    if (envPath is null)
    {
        return;
    }

    foreach (var rawLine in File.ReadAllLines(envPath))
    {
        var line = rawLine.Trim();
        if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
        {
            continue;
        }

        var separatorIndex = line.IndexOf('=', StringComparison.Ordinal);
        if (separatorIndex <= 0)
        {
            continue;
        }

        var key = line[..separatorIndex].Trim();
        var value = line[(separatorIndex + 1)..].Trim();

        // Do not override vars already provided by the environment/launch profile.
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(key)))
        {
            Environment.SetEnvironmentVariable(key, value);
        }
    }
}