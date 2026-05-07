using Pulse.Contracts.Checks;
using Pulse.Server.Checks.Entities;
using Pulse.Server.Checks.Repositories;
using Pulse.Server.Common;

namespace Pulse.Server.Checks.CreateCheck;

public class CreateCheckCommandHandler : IEndpointHandler
{
    private readonly ICheckRepository _repository;

    public CreateCheckCommandHandler(ICheckRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> HandleAsync(CreateCheckRequest request, CancellationToken ct)
    {
        var check = new Check(name: request.Name,
                              type: request.Type,
                              target: request.Target,
                              intervalSeconds: request.IntervalSeconds,
                              timeoutMs: request.TimeoutMs,
                              enabled: true);

        await _repository.AddAsync(check, ct);

        await _repository.SaveChangesAsync(ct);

        return check.Id;
    }
}
