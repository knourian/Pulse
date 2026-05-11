using Pulse.Dashboard;

using Serilog;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "production";
var appName = Assembly.GetExecutingAssembly().GetName().Name;
var serviceVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0";

var appNameVersioned = $"{appName}-{environment} ({serviceVersion})";

Log.Logger = new LoggerConfiguration()
    .Enrich.WithProperty("service", appName)
    .Enrich.WithProperty("environment", environment)
    .WriteTo.Console()
    .CreateLogger();

Log.Information("{AppName} Starting Up....", appNameVersioned);

LoadDotEnv();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseDefaultServiceProvider((context, options) =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

    var app = builder.ConfigureServices()
                     .ConfigurePipeline();


    await app.StartAsync();

    await app.WaitForShutdownAsync();
}

catch (Exception ex)
{
    Log.Fatal(ex, "{AppName} terminated unexpectedly", appNameVersioned);
}
finally
{
    Log.Information("{AppName} Shut down complete", appName);
    await Log.CloseAndFlushAsync();
}


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