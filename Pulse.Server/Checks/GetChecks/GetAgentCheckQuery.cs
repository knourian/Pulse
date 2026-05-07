using Pulse.Contracts.Common;
using Pulse.Server.Common;

namespace Pulse.Server.Checks.GetChecks;

public class GetAgentCheckQuery : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Checks.GetAgentCheck, async (
           HttpContext context,
           GetAgentCheckQueryHandler handler,
           CancellationToken ct) =>
        {
            var result = await handler.HandleAsync(context, ct);
            return Results.Ok(result);
        });
    }
}
