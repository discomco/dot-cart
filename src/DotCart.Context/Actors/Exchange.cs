using System.Collections.Immutable;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using static System.Threading.Tasks.Task;

namespace DotCart.Context.Actors;

public static partial class Inject
{
    public static IServiceCollection AddSingletonExchange(this IServiceCollection services)
    {
        return services
            .AddSingleton<IExchange, Exchange>();
    }
}

[Name(AttributeConstants.ExchangeName)]
internal class Exchange : ActiveComponent, IExchange
{
    private readonly object _subMutex = new();

    private ImmutableDictionary<string, ImmutableList<IActorB>> _topics =
        ImmutableDictionary<string, ImmutableList<IActorB>>.Empty;

    public void Subscribe(string topic, IActorB consumer)
    {
        lock (_subMutex)
        {
            if (!_topics.ContainsKey(topic))
                _topics = _topics.Add(topic, ImmutableList<IActorB>.Empty);
            var lst = _topics[topic];
            lst = lst.All(a => a.Name != consumer.Name)
                ? lst.Add(consumer)
                : lst;
            _topics = _topics.SetItem(topic, lst);
        }
    }


    public void Unsubscribe(string topic, IActorB consumer)
    {
        if (!_topics.ContainsKey(topic))
            return;
        _topics[topic].Remove(consumer);
    }

    public async Task Publish(string topic, IMsg msg, CancellationToken cancellationToken = default)
    {
        var consumers = _topics
            .Where(slot => slot.Key == topic)
            .SelectMany(slot => slot.Value);
        if (!consumers.Any()) return;
        foreach (var consumer in consumers)
            await consumer.HandleCast(msg, cancellationToken);
    }

    public Task HandleCast(IMsg msg, CancellationToken cancellationToken = default)
    {
        return CompletedTask;
    }

    public Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {
        return (Task<IMsg>)CompletedTask;
    }

    public override void Dispose()
    {
        if (_topics != null)
            _topics.Clear();
        Log.Information($"{AppFacts.Stopped} [{GetType().Name}]");
    }

    protected override Task CleanupAsync(CancellationToken cancellationToken)
    {
        return CompletedTask;
    }

    protected override Task PrepareAsync(CancellationToken cancellationToken = default)
    {
        return CompletedTask;
    }

    protected override Task StartActingAsync(CancellationToken cancellationToken = default)
    {
        return CompletedTask;
        // return Run(() =>
        // {
        //     Log.Information($"{AppFacts.Started} [{GetType().Name}]");
        // }, cancellationToken);
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken)
    {
        return CompletedTask;
//        return Run(() => { Log.Information($"{AppVerbs.Stopping} [{GetType().Name}]"); }, cancellationToken);
    }
}