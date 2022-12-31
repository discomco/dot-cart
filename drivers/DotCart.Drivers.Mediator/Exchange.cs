using System.Collections.Immutable;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Serilog;
using static System.Threading.Tasks.Task;

namespace DotCart.Drivers.Mediator;

[Name(AttributeConstants.ExchangeName)]
internal class Exchange : ActiveComponent, IExchange
{
    private readonly object _subMutex = new();

    private ImmutableDictionary<string, ImmutableList<IActor>> _topics =
        ImmutableDictionary<string, ImmutableList<IActor>>.Empty;

    public void Subscribe(string topic, IActor consumer)
    {
        lock (_subMutex)
        {
            if (!_topics.ContainsKey(topic))
                _topics = _topics.Add(topic, ImmutableList<IActor>.Empty);
            var lst = _topics[topic];
            lst = lst.All(a => a.Name != consumer.Name)
                ? lst.Add(consumer)
                : lst;
            _topics = _topics.SetItem(topic, lst);
        }
    }


    public void Unsubscribe(string topic, IActor consumer)
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

    public void Dispose()
    {
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
        return Run(() =>
        {
            var started = "STARTED".AsFact();
            Log.Information($"{started} [{GetType().Name}]");
        }, cancellationToken);
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken)
    {
        var stopping = "STOPPING".AsVerb();
        return Run(() => { Log.Information($"{stopping} [{GetType().Name}]"); }, cancellationToken);
    }
}