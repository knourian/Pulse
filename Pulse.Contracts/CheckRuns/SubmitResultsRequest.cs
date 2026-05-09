namespace Pulse.Contracts.CheckRuns;

public class SubmitResultsRequest
{
    public List<CheckResultDto> Results { get; set; } = new();
}