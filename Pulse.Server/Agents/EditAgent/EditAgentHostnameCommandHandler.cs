using Pulse.Server.Agents.Repositories;
using Pulse.Server.Common;

namespace Pulse.Server.Agents.EditAgent;

public class EditAgentHostnameCommandHandler : IEndpointHandler
{
    private readonly IAgentRepository _repository;

    public EditAgentHostnameCommandHandler(IAgentRepository repository)
    {
        _repository = repository;
    }

    internal async Task HandleAsync(string id, string hostName, CancellationToken ct)
    {
        var agent = await _repository.GetByIdAsync(id, ct);

        if (agent == null)
        {
            throw new InvalidOperationException("Check not found");
        }

        agent.UpdateHostName(hostName);

        await _repository.SaveChangesAsync(ct);
    }
}
