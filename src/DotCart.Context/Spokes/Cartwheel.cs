using DotCart.Abstractions.Actors;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DotCart.Context.Spokes;

public class Cartwheel : BackgroundService
{
    private readonly IEnumerable<IActorB> _actors;
    private readonly IExchange _exchange;
    private readonly IProjector _projector;

    private CancellationTokenSource _cts;
    // private readonly IEnumerable<ISpokeB> _spokes;

    public Cartwheel(
        IExchange exchange
        , IProjector projector
        , IEnumerable<IActorB> actors
    )
    {
        _exchange = exchange;
        _projector = projector;
        _actors = actors;
        // _spokes = spokes;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        await StartExchangeAsync(_cts.Token);
        await StartProjectorAsync(_cts.Token);
        await StartActorsAsync(_cts.Token);
        base.StartAsync(_cts.Token);
    }

    private async Task StartActorsAsync(CancellationToken ctsToken)
    {
        foreach (var actor in _actors)
            await actor.Activate(ctsToken);
    }


    private async Task StartProjectorAsync(CancellationToken cancellationToken)
    {
        while (_projector.Status != ComponentStatus.Active || _exchange.Status != ComponentStatus.Active)
        {
            Log.Information("Attempting to start projector");
            _projector.Activate(cancellationToken).ConfigureAwait(false);
            if (_projector.Status != ComponentStatus.Active)
            {
                Log.Information("Projector is not running, waiting 1 second");
                await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                Log.Information($"Projector [{_projector.GetType()}] is running");
            }
        }
    }


    private async Task StartExchangeAsync(CancellationToken cancellationToken)
    {
        while (_exchange.Status != ComponentStatus.Active)
        {
            Log.Information("Attempting to start exchange");
            _exchange.Activate(cancellationToken).ConfigureAwait(false);
            if (_exchange.Status == ComponentStatus.Active) continue;
            Log.Information("Exchange is not running, waiting 1 sec.");
            await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
        }

        Log.Information("Attempt to start Exchange succeeded");
    }


    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            while (!stoppingToken.IsCancellationRequested) Thread.Sleep(1000);
        }, stoppingToken);
    }

    public override void Dispose()
    {
        _cts.Dispose();
        base.Dispose();
    }
}