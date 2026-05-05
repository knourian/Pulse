namespace Pulse.Dashboard.Models;

public sealed class AppSetting
{
    public PulseServerOptions PulseServer { get; set; } = new();
}
