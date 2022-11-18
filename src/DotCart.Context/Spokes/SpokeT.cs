using System.Collections.Immutable;
using DotCart.Context.Abstractions;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DotCart.Context.Spokes;

public abstract class SpokeT<TSpoke> : BackgroundService, ISpokeT<TSpoke> where TSpoke : ISpokeT<TSpoke>
{
    private readonly IExchange _exchange;

    private readonly IProjector _projector;

    private bool _allActorsUp;

    private IImmutableDictionary<string, IActor<TSpoke>> _reactors =
        ImmutableDictionary<string, IActor<TSpoke>>.Empty;

    protected SpokeT(IExchange exchange, IProjector projector)
    {
        _exchange = exchange;
        _projector = projector;
        Status = ComponentStatus.Inactive;
    }

    public ComponentStatus Status { get; set; }


    public void InjectActors(params IActor<TSpoke>[] reactors)
    {
        foreach (var actor in reactors)
        {
            string actualKey;
            var exists = _reactors.TryGetKey(actor.Name, out actualKey);
            if (exists) continue;
            _reactors = _reactors.Add(actor.Name, actor);
            actor.SetSpoke(this);
        }
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        Log.Information($"Spoke:[{GetType()}] ~> Starting");
        await ActivateExchangeAsync(cancellationToken).ConfigureAwait(false);
        await ActivateActors(cancellationToken).ConfigureAwait(false);
        StartProjectorAsync(cancellationToken).ConfigureAwait(false);
    }


    private async Task ActivateActors(CancellationToken cancellationToken)
    {
        while (!_allActorsUp)
            foreach (var reactor in _reactors)
            {
                while (reactor.Value.Status != ComponentStatus.Active)
                {
                    Log.Information($"Attempting to start Actor [{reactor.Value.GetType().Name}]");
                    reactor.Value.Activate(cancellationToken).ConfigureAwait(false);
                    await Task.Delay(20, cancellationToken).ConfigureAwait(false);
                }

                _allActorsUp = ScanActors();
            }
    }

    private bool ScanActors()
    {
        return _reactors.All(actor => actor.Value.Status == ComponentStatus.Active);
    }

    private async Task ActivateExchangeAsync(CancellationToken cancellationToken)
    {
        while (_exchange.Status != ComponentStatus.Active)
        {
            Log.Information("Attempting to start exchange");
            _exchange.Activate(cancellationToken).ConfigureAwait(false);
            if (_exchange.Status != ComponentStatus.Active)
            {
                Log.Information("Exchange is not running, waiting 1 sec.");
                await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                Log.Information("Attempt to start Exchange succeeded");
            }
        }
    }


    private async Task StartProjectorAsync(CancellationToken cancellationToken)
    {
        if (_projector == null)
            return;
        while (_projector.Status != ComponentStatus.Active)
        {
            Log.Information("Attempting to start projector");
            await _projector.Activate(cancellationToken).ConfigureAwait(false);
            if (_projector.Status != ComponentStatus.Active)
            {
                Log.Information("Projector is not running, waiting 1 second");
                await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                Log.Information("Projector is running");
            }
        }
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
}