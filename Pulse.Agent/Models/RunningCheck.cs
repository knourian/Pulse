using Pulse.Contracts.Checks;

namespace Pulse.Agent.Models;

public class RunningCheck
{
    public CheckDefinitionDto Definition { get; set; }
    public CancellationTokenSource Cts { get; set; }
    public Task Task { get; set; }
}
