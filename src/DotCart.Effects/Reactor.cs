using DotCart.Contract;
using Microsoft.Extensions.Hosting;

namespace DotCart.Effects;

public abstract class Reactor : BackgroundService, IReactor
{
    
    public bool IsRunning { get; private set; }
    
    public abstract Task HandleAsync(IMsg msg);
    protected abstract Task StartReactingAsync(CancellationToken cancellationToken);
    protected abstract Task StopReactingAsync(CancellationToken cancellationToken);
    
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        IsRunning = true;
        return StartReactingAsync(cancellationToken);
    }

    public void SetSpoke(ISpoke spoke)
    {
        spoke.Inject(this);
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Run(() =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
            }
        }, stoppingToken).ConfigureAwait(false);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        IsRunning = false;
        return StopReactingAsync(cancellationToken);
    }
}