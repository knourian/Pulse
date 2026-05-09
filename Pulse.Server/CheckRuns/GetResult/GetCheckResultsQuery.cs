using Pulse.Server.Common;

namespace Pulse.Server.CheckRuns.GetResult;

public class GetCheckResultsQuery : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/checks/{checkId}/results", async (
            string checkId,
            GetCheckResultsQueryHandler handler,
            CancellationToken ct) =>
        {
            var results = await handler.HandleAsync(checkId, ct);

            return Results.Ok(results);
        });
    }
}
