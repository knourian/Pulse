using System.Net.Sockets;

using Pulse.Agent.Contracts;
using Pulse.Agent.Helpers;
using Pulse.Contracts.Checks;
using Pulse.Contracts.Results;

namespace Pulse.Agent.Engines;

public class TcpCheckExecutor : ICheckExecutor
{
    public async Task<CheckResultDto> ExecuteAsync(CheckDefinitionDto check, CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            var (host, port) = Parse(check.Target);

            using var client = new TcpClient();

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(check.TimeoutMs);

            await client.ConnectAsync(host, port, cts.Token);

            sw.Stop();

            return CheckResultFactory.Success(check.Id, sw.ElapsedMilliseconds);
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

    private static (string host, int port) Parse(string target)
    {
        // expected: "host:port"
        var parts = target.Split(':');

        if (parts.Length != 2 || !int.TryParse(parts[1], out var port))
        {
            throw new InvalidOperationException($"Invalid TCP target: {target}");
        }

        return (parts[0], port);
    }
}
