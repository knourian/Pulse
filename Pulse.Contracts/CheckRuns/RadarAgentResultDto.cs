namespace Pulse.Contracts.CheckRuns;

public class RadarAgentResultDto
{
    public string AgentId { get; set; }
    public string AgentName { get; set; }

    public bool IsSuccess { get; set; }

    public long ResponseTimeMs { get; set; }

    public DateTime TimestampUtc { get; set; }
    public string Error { get; set; }
}
