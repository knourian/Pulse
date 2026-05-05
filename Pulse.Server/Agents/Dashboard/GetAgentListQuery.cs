using Pulse.Contracts.Common;
using Pulse.Server.Common;

namespace Pulse.Server.Agents.Dashboard;

public class GetAgentListQuery : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Agents.GetList, async (GetAgentListQueryHandler handler, CancellationToken ct) =>
        {
            var response = await handler.HandleAsync(ct);
            return Results.Ok(response);
        });
    }
}
