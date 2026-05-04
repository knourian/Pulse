namespace Pulse.Server.Common;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
