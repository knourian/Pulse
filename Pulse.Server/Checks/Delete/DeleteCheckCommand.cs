using Pulse.Server.Common;

namespace Pulse.Server.Checks.Delete;

public class DeleteCheckCommand : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/checks/{id}", async (string id, DeleteCheckCommandHandler handler, CancellationToken ct) =>
        {
            await handler.HandleAsync(id, ct);

            return Results.NoContent();
        });
    }
}
