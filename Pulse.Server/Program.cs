using Pulse.Server;

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

try
{
    var builder = WebApplication.CreateBuilder(args);


    builder.Host.UseSerilog((context, lc) =>
    {
        lc.ReadFrom.Configuration(context.Configuration);
    });

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
catch (Microsoft.Extensions.Hosting.HostAbortedException)
{
    // Expected when EF Core tools run design-time operations (e.g., Update-Database).
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