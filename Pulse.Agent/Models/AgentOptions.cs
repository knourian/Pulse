namespace Pulse.Agent.Models;

public sealed class AgentOptions
{
    public int HeartbeatSeconds { get; set; } = 60;

    public int RegistrationMaxAttempts { get; set; } = 5;

    public int RegistrationRetrySeconds { get; set; } = 5;

    [MinLength(16)]
    public string IdentityEncryptionKey { get; set; } = string.Empty;
}
