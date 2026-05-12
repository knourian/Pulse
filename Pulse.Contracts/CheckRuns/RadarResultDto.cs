namespace Pulse.Contracts.CheckRuns;

public class RadarResultDto
{
    public bool IsSuccess { get; set; }

    public long ResponseTimeMs { get; set; }

    public string Error { get; set; }

    public DateTime TimestampUtc { get; set; }
}
