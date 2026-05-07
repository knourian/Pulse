using Pulse.Contracts.Common;
using Pulse.Server.Common;

namespace Pulse.Server.Checks.GetChecks;

public class GetChecksQuery : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Checks.GetCheck, async (GetChecksQueryHandler handler, CancellationToken ct) =>
        {
            var result = await handler.HandleAsync(ct);
            return Results.Ok(result);
        });
    }
}
