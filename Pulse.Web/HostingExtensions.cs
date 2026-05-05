using Pulse.Web.Components;
using Pulse.Web.Models;

namespace Pulse.Web;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorComponents()
           .AddInteractiveServerComponents();


        builder.Services
            .AddOptions<AppSetting>()
            .Bind(builder.Configuration)
            .Validate(s => Uri.TryCreate(s.PulseServer.BaseUrl, UriKind.Absolute, out _),
                            "Invalid 'PulseServer:BaseUrl'.")
            .ValidateOnStart();


        return builder.Build();
    }


    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();


        return app;
    }
}
