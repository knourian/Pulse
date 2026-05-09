using System.Net.NetworkInformation;

using Pulse.Agent.Contracts;
using Pulse.Agent.Helpers;
using Pulse.Contracts.CheckRuns;
using Pulse.Contracts.Checks;

namespace Pulse.Agent.Engines;

public class PingCheckExecutor : ICheckExecutor
{
    public async Task<CheckResultDto> ExecuteAsync(CheckDefinitionDto check, CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            using var ping = new Ping();

            var reply = await ping.SendPingAsync(
                check.Target,
                check.TimeoutMs
            );

            sw.Stop();

            if (reply.Status == IPStatus.Success)
            {
                return CheckResultFactory.Success(check.Id, sw.ElapsedMilliseconds);
            }

            return CheckResultFactory.Failure(
                check.Id,
                sw.ElapsedMilliseconds,
                reply.Status.ToString()
            );
        }
        catch (Exception ex)
        {
            sw.Stop();

            return CheckResultFactory.Failure(
                check.Id,
                sw.ElapsedMilliseconds,
                ex.Message
            );
        }
    }
}