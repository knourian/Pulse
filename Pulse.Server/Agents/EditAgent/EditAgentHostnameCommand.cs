using Pulse.Contracts.Agents;
using Pulse.Server.Common;

namespace Pulse.Server.Agents.EditAgent;

public class EditAgentHostnameCommand : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/agents/{id}", async (string id, EditAgentRequest request, EditAgentHostnameCommandHandler handler, CancellationToken ct) =>
        {
            if (string.IsNullOrWhiteSpace(request.Hostname))
            {
                return Results.BadRequest("Hostname is required");
            }

            await handler.HandleAsync(id, request.Hostname, ct);
            return Results.NoContent();
        });
    }
}
