using System.Collections.Immutable;
using DotCart.Core;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DotCart.Abstractions.Actors;

public abstract class SpokeT<TSpoke> : BackgroundService, ISpokeT<TSpoke> where TSpoke : ISpokeT<TSpoke>
{
    private readonly IExchange _exchange;

    private readonly IProjector _projector;

    private IImmutableDictionary<string, IActor<TSpoke>> _actors =
        ImmutableDictionary<string, IActor<TSpoke>>.Empty;

    private bool _allActorsUp;

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
            var exists = _actors.TryGetKey(actor.Name, out actualKey);
            if (exists) continue;
            _actors = _actors.Add(actor.Name, actor);
            actor.SetSpoke(this);
        }
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var starting = "STARTING".AsVerb();
        Log.Information($"{starting} Spoke:[{NameAtt.Get<TSpoke>()}]");
        await ActivateExchangeAsync(cancellationToken).ConfigureAwait(false);
        await ActivateActors(cancellationToken).ConfigureAwait(false);
        StartProjectorAsync(cancellationToken).ConfigureAwait(false);
        base.StartAsync(cancellationToken).ConfigureAwait(false);
    }


    private async Task ActivateActors(CancellationToken cancellationToken)
    {
        while (!_allActorsUp)
            foreach (var actor in _actors)
            {
                while (actor.Value.Status != ComponentStatus.Active)
                {
                    var starting = "STARTING".AsVerb();
                    Log.Information($"{starting} [{NameAtt.Get(this)}]");
                    actor.Value.Activate(cancellationToken).ConfigureAwait(false);
                    await Task.Delay(20, cancellationToken).ConfigureAwait(false);
                }

                _allActorsUp = ScanActors();
            }
    }

    private bool ScanActors()
    {
        return _actors.All(actor => actor.Value.Status == ComponentStatus.Active);
    }

    private async Task ActivateExchangeAsync(CancellationToken cancellationToken)
    {
        while (_exchange.Status != ComponentStatus.Active)
        {
            var activating = "ACTIVATING".AsVerb();
            Log.Information($"{activating} [{NameAtt.Get(_exchange)}]");
            _exchange.Activate(cancellationToken).ConfigureAwait(false);
            if (_exchange.Status != ComponentStatus.Active)
            {
                var waiting = "WAIT_1S".AsVerb();
                Log.Information($"{waiting} [{NameAtt.Get(_exchange)}]...");
                await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                var started = "STARTED".AsFact();
                Log.Information($"{started} [{NameAtt.Get(_exchange)}]");
            }
        }
    }


    private async Task StartProjectorAsync(CancellationToken cancellationToken)
    {
        if (_projector == null)
            return;
        while (_projector.Status != ComponentStatus.Active)
        {
            var starting = "STARTING".AsVerb();
            Log.Information($"{starting} Projector [{_projector.Name}]");
            await _projector.Activate(cancellationToken).ConfigureAwait(false);
            if (_projector.Status != ComponentStatus.Active)
            {
                var waiting = "WAITING_1S".AsVerb();
                Log.Information($"{waiting} Projector [{_projector.Name}]");
                await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                var running = "RUNNING".AsVerb();
                Log.Information($"{running} Projector [{_projector.Name}]");
            }
        }
    }


    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(async () =>
        {
            Status = ComponentStatus.Active;
            Log.Debug($"::RUNNING:: Spoke: [{NameAtt.Get<TSpoke>()}");
            while (!stoppingToken.IsCancellationRequested) Thread.Sleep(1000);
        }, stoppingToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        Status = ComponentStatus.Inactive;
        Log.Debug($"::STOPPED:: Spoke: [{NameAtt.Get<TSpoke>()}");
        return base.StopAsync(cancellationToken);
    }
}