namespace Pulse.Contracts.CheckRuns;

public class RadarAgentDto
{
    public string AgentId { get; set; } = default!;

    public string AgentName { get; set; } = default!;

    public DateTime LastHeartbeatUtc { get; set; }
    public bool IsOnline => LastHeartbeatUtc >= DateTime.UtcNow.AddMinutes(-5);
    public List<RadarResultDto> Results { get; set; } = [];
}
