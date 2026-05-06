namespace Pulse.Contracts.Checks;

public class CheckDefinitionDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public CheckType Type { get; set; }

    public string Target { get; set; }

    public int IntervalSeconds { get; set; }
    public int TimeoutMs { get; set; }
}
