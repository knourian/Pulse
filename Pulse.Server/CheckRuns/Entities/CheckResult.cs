namespace Pulse.Server.CheckRuns.Entities;

public class CheckResult
{
    public long Id { get; set; }

    public string CheckId { get; set; } = default!;
    public string AgentId { get; set; } = default!;

    public DateTime TimestampUtc { get; set; }

    public bool IsSuccess { get; set; }
    public long ResponseTimeMs { get; set; }

    public string Error { get; set; }
}
