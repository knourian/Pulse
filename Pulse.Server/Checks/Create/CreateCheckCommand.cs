using Pulse.Contracts.Checks;
using Pulse.Contracts.Common;
using Pulse.Server.Common;

namespace Pulse.Server.Checks.Create;

public class CreateCheckCommand : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiRoutes.Checks.CreateCheck, async (CreateCheckRequest request, CreateCheckCommandHandler handler, CancellationToken ct) =>
        {
            var id = await handler.HandleAsync(request, ct);

            return Results.Created($"/api/checks/{id}", null);
        });
    }
}
