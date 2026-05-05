namespace Pulse.Web.Models;

public sealed class PulseServerOptions
{
    [Required]
    public string BaseUrl { get; set; } = string.Empty;
}