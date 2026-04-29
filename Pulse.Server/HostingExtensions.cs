namespace Pulse.Server;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization();
        builder.Services.AddOpenApi();
        return builder.Build();
    }


    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.Map("/", () =>
                    $"{Assembly.GetExecutingAssembly().GetName().Name} Version " +
                    $"{Assembly.GetExecutingAssembly().GetName().Version} is ready ({app.Environment.EnvironmentName})");

        app.UseAuthorization();

        return app;
    }



}
