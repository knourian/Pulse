namespace Pulse.Contracts.Checks;

public class CheckDto
{
    public string Id { get; set; }
    public string Name { get; set; }

    public CheckType Type { get; set; }

    public string Target { get; set; }

    public int IntervalSeconds { get; set; }

    public int TimeoutMs { get; set; }

    public bool Enabled { get; set; }

    public DateTime CreatedUtc { get; set; }
}
