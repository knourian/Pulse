namespace Pulse.Agent.Models;

public sealed class AppSetting
{
    public AgentOptions Agent { get; set; } = new();
    public PulseServerOptions PulseServer { get; set; } = new();
}
