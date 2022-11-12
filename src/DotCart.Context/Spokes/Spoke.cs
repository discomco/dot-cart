using System.Collections.Immutable;
using DotCart.Context.Effects;
using Microsoft.Extensions.Hosting;

namespace DotCart.Context.Spokes;

public abstract class Spoke<TSpoke> : BackgroundService, ISpoke<TSpoke> where TSpoke : ISpoke<TSpoke>
{
    
    protected IImmutableDictionary<string, IReactor<TSpoke>> _reactors =ImmutableDictionary<string, IReactor<TSpoke>>.Empty;

    public void Inject(params IReactor<TSpoke>[] reactors)
    {
        foreach (var effect in reactors)
        {
            string actualKey;
            var exists = _reactors.TryGetKey(effect.Name, out actualKey);
            if (!exists)
                _reactors = _reactors.Add(effect.Name, effect);
        }
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            foreach (var reactor in _reactors)
            {
                reactor.Value.Activate(cancellationToken);
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