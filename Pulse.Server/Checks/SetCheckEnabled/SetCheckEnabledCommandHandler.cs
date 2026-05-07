using Pulse.Server.Checks.Repositories;
using Pulse.Server.Common;

namespace Pulse.Server.Checks.SetCheckEnabled;

public class SetCheckEnabledCommandHandler : IEndpointHandler
{
    private readonly ICheckRepository _repository;

    public SetCheckEnabledCommandHandler(ICheckRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(string id, bool enabled, CancellationToken ct)
    {
        var check = await _repository.GetByIdAsync(id, ct);

        if (check == null)
        {
            throw new InvalidOperationException("Check not found");
        }

        check.SetEnabled(enabled);

        await _repository.SaveChangesAsync(ct);
    }
}
