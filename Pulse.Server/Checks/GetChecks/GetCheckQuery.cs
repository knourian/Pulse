using Pulse.Contracts.Common;
using Pulse.Server.Common;

namespace Pulse.Server.Checks.GetChecks;

public class GetCheckQuery : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Checks.Get, async (
           HttpContext context,
           GetCheckQueryHandler handler,
           CancellationToken ct) =>
        {
            var result = await handler.HandleAsync(context, ct);
            return Results.Ok(result);
        });
    }
}
