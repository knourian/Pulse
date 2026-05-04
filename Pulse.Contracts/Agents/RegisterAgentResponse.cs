namespace Pulse.Contracts.Agents;

public class RegisterAgentResponse
{
    public string AgentId { get; set; } = default!;
    public string ApiKey { get; set; } = default!;
}
