using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using Pulse.Server.Agents.Repositories;
using Pulse.Server.Common;
using Pulse.Server.Data;
using Pulse.Server.Models;

namespace Pulse.Server;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {

        builder.Services.AddOptions<AppSetting>()
                        .Bind(builder.Configuration)
                        .ValidateOnStart();

        builder.Services.AddAuthorization();
        builder.Services.AddOpenApi();
        builder.Services.AddDatabase();

        builder.Services.AddHandlers(typeof(Program).Assembly);
        builder.Services.AddEndpoints(typeof(Program).Assembly);

        return builder.Build();
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var appSetting = serviceProvider.GetRequiredService<IOptions<AppSetting>>().Value;

            options.UseSqlite(appSetting.ConnectionStrings.DefaultConnection);
        });

        services.AddScoped<IAgentRepository, AgentRepository>();


        return services;
    }

    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }


    public static IServiceCollection AddHandlers(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpointHandler)))
            .Select(type => ServiceDescriptor.Scoped(type, type))
            .ToArray();

        services.TryAdd(serviceDescriptors);

        return services;
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

        app.MapEndpoints();

        return app;
    }

    public static IApplicationBuilder MapEndpoints(this WebApplication app, RouteGroupBuilder routeGroupBuilder = null)
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }



}
