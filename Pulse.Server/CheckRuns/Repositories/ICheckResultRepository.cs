using Pulse.Contracts.CheckRuns;
using Pulse.Server.CheckRuns.Entities;

namespace Pulse.Server.CheckRuns.Repositories;

public interface ICheckResultRepository
{
    Task<List<CheckResultDto>> GetRecentByCheckIdAsync(string checkId, int limit, CancellationToken ct);
    Task AddRangeAsync(List<CheckResult> results, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);

    Task<List<RadarCheckDto>> GetRadarAsync(CancellationToken ct);
}
