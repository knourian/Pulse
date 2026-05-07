using Pulse.Contracts.Checks;
using Pulse.Server.Common;

namespace Pulse.Server.Checks.SetCheckEnabled;

public class SetCheckEnabledCommand : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/checks/{id}/enabled", async (string id, SetCheckEnabledRequest request, SetCheckEnabledCommandHandler handler, CancellationToken ct) =>
        {
            await handler.HandleAsync(id, request.Enabled, ct);

            return Results.NoContent();
        });
    }
}
