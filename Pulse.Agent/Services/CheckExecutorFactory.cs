using Pulse.Agent.Contracts;
using Pulse.Agent.Engines;
using Pulse.Contracts.Checks;

namespace Pulse.Agent.Services;

public class CheckExecutorFactory : ICheckExecutorFactory
{
    private readonly IServiceProvider _provider;

    public CheckExecutorFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    public ICheckExecutor Create(CheckType type)
    {
        return type switch
        {
            CheckType.Http => _provider.GetRequiredService<HttpCheckExecutor>(),
            CheckType.Tcp => _provider.GetRequiredService<TcpCheckExecutor>(),
            CheckType.Ping => _provider.GetRequiredService<PingCheckExecutor>(),
            _ => throw new NotSupportedException($"Check type {type} is not supported")
        };
    }
}
