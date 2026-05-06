using Pulse.Contracts.Results;
using Pulse.Server.Agents.Repositories;
using Pulse.Server.CheckRuns.Entities;
using Pulse.Server.CheckRuns.Repositories;
using Pulse.Server.Common;
using Pulse.Server.Common.Extensions;

namespace Pulse.Server.CheckRuns.Ingest;

public class SubmitResultsHandler : IEndpointHandler
{
    private readonly ICheckResultRepository _results;
    private readonly IAgentRepository _agents;

    public SubmitResultsHandler(ICheckResultRepository results, IAgentRepository agents)
    {
        _results = results;
        _agents = agents;
    }
    public async Task HandleAsync(HttpContext context, SubmitResultsRequest request, CancellationToken ct)
    {
        var apiKey = context.GetApiKey();

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new UnauthorizedAccessException();
        }

        var agent = await _agents.GetByApiKeyAsync(apiKey, ct);

        if (agent == null)
        {
            throw new UnauthorizedAccessException();
        }

        var entities = request.Results.Select(x => new CheckResult
        {
            CheckId = x.CheckId,
            AgentId = agent.Id,
            TimestampUtc = x.TimestampUtc,
            IsSuccess = x.IsSuccess,
            ResponseTimeMs = x.ResponseTimeMs,
            Error = x.Error
        }).ToList();

        await _results.AddRangeAsync(entities, ct);

        agent.LastSeenUtc = DateTime.UtcNow;

        await _results.SaveChangesAsync(ct);
    }
}
