using Pulse.Contracts.Agents;
using Pulse.Contracts.Common;
using Pulse.Server.Common;

namespace Pulse.Server.Agents.Register;

public class RegisterAgentEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiRoutes.Agents.Register, async (
            RegisterAgentRequest request,
            RegisterAgentHandler handler,
            CancellationToken ct) =>
        {
            if (string.IsNullOrWhiteSpace(request.Hostname))
            {
                return Results.BadRequest("Hostname is required");
            }

            if (string.IsNullOrWhiteSpace(request.MachineId))
            {
                return Results.BadRequest("MachineId is required");
            }

            var response = await handler.HandleAsync(request, ct);
            return Results.Ok(response);
        });
    }
}
