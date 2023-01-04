using System.Collections.Immutable;
using DotCart.Core;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DotCart.Abstractions.Actors;

public abstract class SpokeT<TSpoke> : BackgroundService, ISpokeT<TSpoke> where TSpoke : ISpokeT<TSpoke>
{
    private readonly IExchange _exchange;

    private readonly IProjector _projector;

    private IImmutableDictionary<string, IActorT<TSpoke>> _actors =
        ImmutableDictionary<string, IActorT<TSpoke>>.Empty;

    private bool _allActorsUp;

    protected SpokeT(IExchange exchange, IProjector projector)
    {
        _exchange = exchange;
        _projector = projector;
        Status = ComponentStatus.Inactive;
    }

    public ComponentStatus Status { get; set; }


    public void InjectActors(params IActorT<TSpoke>[] reactors)
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
        try
        {
            Log.Information($"{AppVerbs.Starting} Spoke:[{NameAtt.Get<TSpoke>()}]");
            await ActivateExchangeAsync(cancellationToken).ConfigureAwait(false);
            await ActivateActors(cancellationToken).ConfigureAwait(false);
            StartProjectorAsync(cancellationToken).ConfigureAwait(false);
            base.StartAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Log.Fatal($"{AppErrors.Error} => {e.InnerAndOuter()}");
            throw;
        }
    }


    private async Task ActivateActors(CancellationToken cancellationToken)
    {
        _allActorsUp = ScanActors();
        while (!_allActorsUp)
            foreach (var actor in _actors)
            {
                var i = 0;
                while (actor.Value.Status != ComponentStatus.Active)
                {
                    i++;
                    Log.Information($"{AppVerbs.Looping("Activate", i)} [{actor.Value.GetType()}]");
                    actor.Value.Activate(cancellationToken).ConfigureAwait(false);
                    await Task.Delay(20, cancellationToken).ConfigureAwait(false);
                }

                _allActorsUp = ScanActors();
            }
    }

    private bool ScanActors()
    {
        return !_actors.Any() || _actors.All(actor => actor.Value.Status == ComponentStatus.Active);
    }

    private async Task ActivateExchangeAsync(CancellationToken cancellationToken)
    {
        while (_exchange.Status != ComponentStatus.Active)
        {
            Log.Information($"{AppVerbs.Activating} [{NameAtt.Get(_exchange)}]");
            _exchange.Activate(cancellationToken).ConfigureAwait(false);
            if (_exchange.Status != ComponentStatus.Active)
            {
                Log.Information($"{AppVerbs.Waiting1s} [{NameAtt.Get(_exchange)}]...");
                Thread.Sleep(1);
//                await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                Log.Information($"{AppFacts.Started} [{NameAtt.Get(_exchange)}]");
            }
        }
    }


    private async Task StartProjectorAsync(CancellationToken cancellationToken)
    {
        var i = 0;
        if (_projector == null)
            return;
        while (_projector.Status != ComponentStatus.Active)
        {
            i++;
            Log.Information($"{AppVerbs.Looping("Activate", i)} Projector [{_projector.Name}]");
            await _projector.Activate(cancellationToken).ConfigureAwait(false);
            if (_projector.Status != ComponentStatus.Active)
            {
                Log.Information($"{AppVerbs.Waiting1s} Projector [{_projector.Name}]");
                await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                Log.Information($"{AppVerbs.Running} Projector [{_projector.Name}]");
            }
        }
    }


    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(async () =>
        {
            Status = ComponentStatus.Active;
            Log.Debug($"{AppVerbs.Running} Spoke: [{NameAtt.Get<TSpoke>()}");
            while (!stoppingToken.IsCancellationRequested)
                Thread.Sleep(1000);
            return Task.CompletedTask;
        }, stoppingToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        Status = ComponentStatus.Inactive;
        Log.Debug($"{AppFacts.Stopped} Spoke: [{NameAtt.Get<TSpoke>()}");
        return base.StopAsync(cancellationToken);
    }
}