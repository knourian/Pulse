using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

using Pulse.Dashboard.Api;
using Pulse.Dashboard.Components;
using Pulse.Dashboard.Models;
using Pulse.Dashboard.Services.Agents;
using Pulse.Dashboard.Services.CheckResults;
using Pulse.Dashboard.Services.Checks;

namespace Pulse.Dashboard;

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

        builder.Services.Configure<DashboardOptions>(builder.Configuration.GetSection("Dashboard"));

        builder.Services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/login";

                options.AccessDeniedPath = "/login";

                options.Cookie.Name = "pulse-dashboard-auth";

                options.ExpireTimeSpan = TimeSpan.FromHours(12);

                options.SlidingExpiration = true;

                options.Cookie.SameSite = SameSiteMode.Strict;

                options.Cookie.HttpOnly = true;
            });

        builder.Services.AddAuthorization();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddCascadingAuthenticationState();


        builder.Services.AddHttpClient<IAgentService, AgentService>(HttpClientConfiguration());

        builder.Services.AddHttpClient<ICheckService, CheckService>(HttpClientConfiguration());

        builder.Services.AddScoped(sp =>
        {
            var nav = sp.GetRequiredService<NavigationManager>();
            return new HttpClient { BaseAddress = new Uri(nav.BaseUri) };
        });

        builder.Services.AddHttpClient<ICheckResultService, CheckResultService>(HttpClientConfiguration());



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

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapAuthEndpoints();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();


        return app;
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
