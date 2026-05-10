using Pulse.Contracts.Common;
using Pulse.Server.Common;

namespace Pulse.Server.CheckRuns.Radar;

public class GetRadarQuery : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Results.Radar, async (GetRadarQueryHandler handler, CancellationToken ct) =>
        {
            var result = await handler.HandleAsync(ct);

            return Results.Ok(result);
        });
    }
}

