namespace Pulse.Contracts.Results;

public class SubmitResultsRequest
{
    public List<CheckResultDto> Results { get; set; } = new();
}