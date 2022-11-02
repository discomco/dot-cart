using DotCart.Contract;
using DotCart.Effects;
using DotCart.Effects.Drivers;
using static System.Threading.Tasks.Task;


namespace DotCart.Drivers.InMem;


public delegate THope GenerateHope<out THope>() 
    where THope: IHope;

/// <summary>
/// MemResponderDriver is an in-memory responder driver mimicker. 
/// </summary>
/// <typeparam name="THope"></typeparam>
public abstract class MemResponderDriver<THope> : IResponderDriver<THope> where THope: IHope
{
    private readonly GenerateHope<THope> _generateHope;
    private IReactor _reactor;

    public MemResponderDriver(GenerateHope<THope> generateHope)
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