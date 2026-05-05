namespace Pulse.Web.Models;

public sealed class AppSetting
{
    public PulseServerOptions PulseServer { get; set; } = new();
}
