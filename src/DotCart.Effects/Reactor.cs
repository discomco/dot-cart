using DotCart.Contract;
using Microsoft.Extensions.Hosting;

namespace DotCart.Effects;

public abstract class Reactor : BackgroundService, IReactor
{
    protected abstract Task StartReactingAsync(CancellationToken cancellationToken);
    protected abstract Task StopReactingAsync(CancellationToken cancellationToken);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Run(() =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
            }
        }, stoppingToken).ConfigureAwait(false);
    }
    
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        return StartReactingAsync(cancellationToken);
    }
  
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return StopReactingAsync(cancellationToken);
    }
    public void SetSpoke(ISpoke spoke)
    {
        spoke.Inject(this);
    }
    public abstract Task HandleAsync(IMsg msg);
}