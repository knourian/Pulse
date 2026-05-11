using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

using Pulse.Dashboard.Helpers;
using Pulse.Dashboard.Models;

namespace Pulse.Dashboard.Api;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/login", async (HttpContext httpContext, IOptions<DashboardOptions> dashboardOptions, LoginRequest request) =>
        {
            var options = dashboardOptions.Value;

            if (!string.Equals(request.Username, options.Username, StringComparison.Ordinal)
                || !PasswordHasher.Verify(request.Password, options.PasswordHash))
            {
                return Results.Unauthorized();
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, request.Username)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Results.Ok();
        })
        .AllowAnonymous();

        app.MapPost("/auth/logout", async (HttpContext httpContext) =>
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.Redirect("/login");
        }).DisableAntiforgery();

        return app;
    }

    private sealed record LoginRequest(string Username, string Password);
}