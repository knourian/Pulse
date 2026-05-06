using Pulse.Contracts.Results;

namespace Pulse.Agent.Helpers;

public static class CheckResultFactory
{
    public static CheckResultDto Success(string checkId, long responseTimeMs)
    {
        return new CheckResultDto
        {
            CheckId = checkId,
            TimestampUtc = DateTime.UtcNow,
            IsSuccess = true,
            ResponseTimeMs = responseTimeMs
        };
    }

    public static CheckResultDto Failure(string checkId, long responseTimeMs, string error)
    {
        return new CheckResultDto
        {
            CheckId = checkId,
            TimestampUtc = DateTime.UtcNow,
            IsSuccess = false,
            ResponseTimeMs = responseTimeMs,
            Error = error
        };
    }
}
