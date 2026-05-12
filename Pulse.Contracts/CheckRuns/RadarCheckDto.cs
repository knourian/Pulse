namespace Pulse.Contracts.CheckRuns;

public class RadarCheckDto
{
    public string CheckId { get; set; }

    public string CheckName { get; set; }
    public List<RadarAgentDto> Agents { get; set; } = [];
}
