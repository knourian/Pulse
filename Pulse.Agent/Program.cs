using Pulse.Agent;

using Serilog;

LoadDotEnv();

var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory
});
Directory.SetCurrentDirectory(AppContext.BaseDirectory);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "production";
var appName = Assembly.GetExecutingAssembly().GetName().Name;
var serviceVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0";


var appNameVersioned = $"{appName}-{environment} ({serviceVersion})";

Log.Logger = new LoggerConfiguration()
    .Enrich.WithProperty("service", appName)
    .Enrich.WithProperty("environment", environment)
    .WriteTo.Console()
    .CreateLogger();

AppDomain.CurrentDomain.UnhandledException += (_, eventArgs) =>
{
    if (eventArgs.ExceptionObject is Exception ex)
    {
        Log.Fatal(ex, "Unhandled exception in process.");
        return;
    }

    Log.Fatal("Unhandled non-exception error in process.");
};

TaskScheduler.UnobservedTaskException += (_, eventArgs) =>
{
    Log.Error(eventArgs.Exception, "Unobserved task exception.");
    eventArgs.SetObserved();
};

builder.Services.Configure<HostOptions>(options =>
{
    options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});



builder.Services.AddSerilog((services, lc) => lc
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(services)
    .Enrich.WithProperty("service", appName)
    .Enrich.WithProperty("environment", environment));

Log.Information("{AppName} Starting Up....", appNameVersioned);


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