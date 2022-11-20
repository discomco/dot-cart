using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using static System.Threading.Tasks.Task;


namespace DotCart.Drivers.InMem;

/// <summary>
///     MemResponderDriver is an in-memory responder driver mimicker.
/// </summary>
/// <typeparam name="THope"></typeparam>
public abstract class MemResponderDriver<THope> : IResponderDriverT<THope> where THope : IHope
{
    private readonly GenerateHope<THope> _generateHope;
    private IActor _actor;

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
                await _actor.HandleCast(hope, cancellationToken);
            }
        }, cancellationToken);
    }

    public Task StopRespondingAsync(CancellationToken cancellationToken)
    {
        return CompletedTask;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void SetActor(IActor actor)
    {
        _actor = actor;
    }

    protected abstract void Dispose(bool disposing);
}