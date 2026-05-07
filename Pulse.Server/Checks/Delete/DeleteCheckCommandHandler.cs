using Pulse.Server.Checks.Repositories;
using Pulse.Server.Common;

namespace Pulse.Server.Checks.Delete;

public class DeleteCheckCommandHandler : IEndpointHandler
{
    private readonly ICheckRepository _repository;

    public DeleteCheckCommandHandler(ICheckRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(string id, CancellationToken ct)
    {
        var check = await _repository.GetByIdAsync(id, ct);

        if (check == null)
        {
            return;
        }

        await _repository.Remove(check);

        await _repository.SaveChangesAsync(ct);
    }
}
