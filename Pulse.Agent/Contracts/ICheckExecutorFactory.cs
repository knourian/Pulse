using Pulse.Contracts.Checks;

namespace Pulse.Agent.Contracts;

public interface ICheckExecutorFactory
{
    ICheckExecutor Create(CheckType type);
}