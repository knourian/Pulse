using Pulse.Agent;

LoadDotEnv();

var builder = Host.CreateApplicationBuilder(args);


builder.RegisterServices();

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

        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(key)))
        {
            Environment.SetEnvironmentVariable(key, value);
        }
    }
}