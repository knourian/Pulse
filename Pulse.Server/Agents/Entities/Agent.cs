namespace Pulse.Server.Agents.Entities;

public class Agent
{
    public string Id { get; set; }
    public string MachineId { get; set; }
    public string Hostname { get; set; }
    public string ApiKey { get; set; }

    public DateTime LastSeenUtc { get; set; }
}