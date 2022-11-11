using DotCart.Client;
using DotCart.Client.Contracts;
using DotCart.Context.Effects;
using DotCart.Context.Effects.Drivers;
using static System.Threading.Tasks.Task;


namespace DotCart.Drivers.InMem;

/// <summary>
///     MemResponderDriver is an in-memory responder driver mimicker.
/// </summary>
/// <typeparam name="THope"></typeparam>
public abstract class MemResponderDriver<THope> : IResponderDriver<THope> where THope : IHope
{
    private readonly GenerateHope<THope> _generateHope;
    private IReactor _reactor;

    protected MemResponderDriver(GenerateHope<THope> generateHope)
    {
        _generateHope = generateHope;
    }

    public Task StartRespondingAsync(CancellationToken cancellationToken)
    {
        return Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(500);
                var hope = _generateHope();
                await _reactor.HandleAsync(hope, cancellationToken);
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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected abstract void Dispose(bool disposing);
}