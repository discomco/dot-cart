using System.Collections.Immutable;
using DotCart.Context.Abstractions;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DotCart.Context.Spokes;

public abstract class SpokeT<TSpoke> : BackgroundService, ISpokeT<TSpoke> where TSpoke : ISpokeT<TSpoke>
{
    private readonly IExchange _exchange;

    private readonly IProjector _projector;

    private IImmutableDictionary<string, IActor<TSpoke>> _reactors =
        ImmutableDictionary<string, IActor<TSpoke>>.Empty;

    protected SpokeT(IExchange exchange, IProjector projector)
    {
        _exchange = exchange;
        _projector = projector;
    }


    public void InjectActors(params IActor<TSpoke>[] reactors)
    {
        foreach (var actor in reactors)
        {
            string actualKey;
            var exists = _reactors.TryGetKey(actor.Name, out actualKey);
            if (!exists)
                _reactors = _reactors.Add(actor.Name, actor);
        }
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.Run(async () =>
        {
            Log.Information($"Spoke:[{GetType()}] ~> Starting");
            while (!_exchange.IsRunning)
            {
                _exchange.Activate(cancellationToken).ConfigureAwait(false);
                if(!_exchange.IsRunning)
                    await Task.Delay(100, cancellationToken).ConfigureAwait(false);
            }

            foreach (var reactor in _reactors)
            {
                await Task.Delay(20, cancellationToken).ConfigureAwait(false);
                reactor.Value.Activate(cancellationToken).ConfigureAwait(false);
            }

            while (!_projector.IsRunning)
            {
                _projector.Activate(cancellationToken).ConfigureAwait(false);
                if(!_projector.IsRunning)
                    await Task.Delay(100, cancellationToken).ConfigureAwait(false);
            }
        }, cancellationToken);
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