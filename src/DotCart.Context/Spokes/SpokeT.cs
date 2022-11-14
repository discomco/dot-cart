using System.Collections.Immutable;
using DotCart.Context.Abstractions;
using Microsoft.Extensions.Hosting;

namespace DotCart.Context.Spokes;

public abstract class SpokeT<TSpoke> : BackgroundService, ISpokeT<TSpoke> where TSpoke : ISpokeT<TSpoke>
{
    private readonly IExchange _exchange;

    protected IImmutableDictionary<string, IActor<TSpoke>> _reactors =
        ImmutableDictionary<string, IActor<TSpoke>>.Empty;

    public SpokeT(IExchange exchange)
    {
        _exchange = exchange;
    }


    public void Inject(params IActor<TSpoke>[] reactors)
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
            if (!_exchange.IsRunning)
            {
                _exchange.Activate(cancellationToken);
                Task.Delay(100);
            }

            foreach (var reactor in _reactors)
            {
                Task.Delay(20, cancellationToken);
                await reactor.Value.Activate(cancellationToken);
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