namespace Pulse.Contracts.Checks;

public class CreateCheckRequest
{
    public string Name { get; set; }

    public CheckType Type { get; set; }

    public string Target { get; set; }

    public int IntervalSeconds { get; set; }

    public int TimeoutMs { get; set; }
}
