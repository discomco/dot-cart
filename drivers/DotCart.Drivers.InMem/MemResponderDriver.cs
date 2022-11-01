using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Effects;
using DotCart.Effects.Drivers;
using Microsoft.Extensions.DependencyInjection;
using static System.Threading.Tasks.Task;


namespace DotCart.Drivers.InMem;

public static partial class Inject
{
    public static IServiceCollection AddMemResponderDriver(this IServiceCollection services)
    {
        return services
            .AddSingleton<IResponderDriver, MemResponderDriver>();
    }
}


public delegate IHope GenerateHope();

public class MemResponderDriver : IResponderDriver
{
    private readonly GenerateHope _generateHope;
    private IReactor _reactor;

    public MemResponderDriver(GenerateHope generateHope)
    {
        _generateHope = generateHope;
    }


    public Task StartRespondingAsync(CancellationToken cancellationToken)
    {
        return Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(10_000);
                var hope = _generateHope();
                await _reactor.HandleAsync(hope);
            }
        }, cancellationToken);
    }

    public Task StopRespondingAsync(CancellationToken cancellationToken)
    {
        return CompletedTask;
    }


    public void SetReactor(IReactor reactor)
    {
        _reactor = reactor;
    }
}