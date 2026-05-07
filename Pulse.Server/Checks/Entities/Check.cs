using Pulse.Contracts.Checks;
using Pulse.Server.Common.Extensions;

namespace Pulse.Server.Checks.Entities;

public class Check
{

    private Check()
    {
        Id = Guid.NewGuid().ToString("N");
        CreatedUtc = DateTime.UtcNow;
    }

    public Check(string name, CheckType type, string target, int intervalSeconds, int timeoutMs, bool enabled) : this()
    {
        Name = name;
        Type = type;
        Target = target;
        IntervalSeconds = intervalSeconds;
        TimeoutMs = timeoutMs;
        Enabled = enabled;
        UpdateSignature();
    }
    public string Id { get; private set; }

    public string Name { get; private set; }
    public CheckType Type { get; private set; }
    public string Target { get; private set; }
    public int IntervalSeconds { get; private set; }

    public int TimeoutMs { get; private set; }
    public bool Enabled { get; private set; }
    public string Signature { get; private set; }

    public DateTime CreatedUtc { get; private set; }
    public DateTime? UpdatedUtc { get; private set; }

    public void SetEnabled(bool enabled)
    {
        Enabled = enabled;
        Touch();
    }

    private void UpdateSignature()
    {
        var target = Target.UrlNormalize();
        Signature = $"{Type}:{target}";
    }

    private void Touch()
    {
        UpdatedUtc = DateTime.UtcNow;
    }
}
