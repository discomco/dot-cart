using System.Collections.Immutable;
using DotCart.Context.Abstractions;
using DotCart.Contract.Dtos;
using Serilog;
using static System.Threading.Tasks.Task;

namespace DotCart.Drivers.Mediator;

internal class Exchange : ActiveComponent, IExchange
{
    private ImmutableDictionary<string, List<IActor>> _inbox = ImmutableDictionary<string, List<IActor>>.Empty;

    public void Subscribe(string topic, IActor consumer)
    {
        if (!_inbox.ContainsKey(topic))
            _inbox = _inbox.Add(topic, new List<IActor>());
        var lst = _inbox[topic];
        lst.Add(consumer);
        _inbox = _inbox.SetItem(topic, lst);
    }

    public void Unsubscribe(string topic, IActor consumer)
    {
        if (!_inbox.ContainsKey(topic)) return;
        _inbox[topic].Remove(consumer);
    }

    public Task Publish(string topic, IMsg msg, CancellationToken cancellationToken = default)
    {
        return Run(() =>
        {
            var consumers = _inbox
                .Where(slot => slot.Key == topic)
                .SelectMany(slot => slot.Value)
                .AsParallel();
            if (!consumers.Any()) return CompletedTask;
            Parallel.ForEachAsync(consumers, cancellationToken, (actor, token) =>
            {
                actor.HandleCast(msg, token);
                return default;
            });
            return CompletedTask;
        }, cancellationToken);
    }

    public Task HandleCast(IMsg msg, CancellationToken cancellationToken = default)
    {
        return CompletedTask;
    }

    public Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {
        return (Task<IMsg>)CompletedTask;
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
        return Run(() => { Log.Information($"[{GetType().Name}] ~> Started;"); }, cancellationToken);
        return CompletedTask;
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken)
    {
        return Run(() => { Log.Information($"[{GetType().Name}] ~> Stopping;"); }, cancellationToken);
        return CompletedTask;
    }

    public void Dispose()
    {
        _inbox.Clear();
        Log.Information($"[{GetType().Name}] ~> Stopped.");
    }
}