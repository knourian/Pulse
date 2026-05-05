namespace Pulse.Contracts.Agents;

public class AgentDto
{
    public string Id { get; set; }
    public string Hostname { get; set; }
    public DateTime LastSeenUtc { get; set; }
}
