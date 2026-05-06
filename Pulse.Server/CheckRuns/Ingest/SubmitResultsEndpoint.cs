using Pulse.Contracts.Common;
using Pulse.Contracts.Results;
using Pulse.Server.Common;

namespace Pulse.Server.CheckRuns.Ingest;

public class SubmitResultsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiRoutes.Results.Submit, async (
            HttpContext context,
            SubmitResultsRequest request,
            SubmitResultsHandler handler,
            CancellationToken ct) =>
        {
            await handler.HandleAsync(context, request, ct);
            return Results.Ok();
        });
    }
}
