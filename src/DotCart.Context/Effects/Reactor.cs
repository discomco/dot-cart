using DotCart.Contract.Dtos;
using DotCart.Core;
using Serilog;

namespace DotCart.Context.Effects;

public abstract class Reactor<TSpoke> : IReactor<TSpoke> where TSpoke : ISpoke<TSpoke>
{
    public bool IsRunning { get; private set; }

    public abstract Task HandleAsync(IMsg msg, CancellationToken cancellationToken);

    private Task StartAsync(CancellationToken cancellationToken)
    {
        Log.Information($"Activating Reactor ~> [{Name}]");
        IsRunning = true;
        return StartReactingAsync(cancellationToken);
    }

    public void SetSpoke(TSpoke spoke)
    {
        spoke.Inject(this);
    }

    public string Name => GetType().Name;

    protected abstract Task StartReactingAsync(CancellationToken cancellationToken);
    protected abstract Task StopReactingAsync(CancellationToken cancellationToken);


    public async Task Activate(CancellationToken stoppingToken)
    {
        await StartAsync(stoppingToken).ConfigureAwait(false);
        while (!stoppingToken.IsCancellationRequested) { }
        await StopAsync(stoppingToken).ConfigureAwait(false);
    }

    private async Task StopAsync(CancellationToken cancellationToken)
    {
        IsRunning = false;
        await StopReactingAsync(cancellationToken).ConfigureAwait(false);
        Log.Information($"Reactor [{Name}]  ~> STOPPED.");
    }
}