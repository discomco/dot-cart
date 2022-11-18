using System.Collections.Immutable;
using DotCart.Context.Abstractions;
using DotCart.Contract.Dtos;
using Serilog;
using static System.Threading.Tasks.Task;

namespace DotCart.Drivers.Mediator;

internal class Exchange : ActiveComponent, IExchange
{
    private ImmutableDictionary<string, List<IActor>> _readers = ImmutableDictionary<string, List<IActor>>.Empty;

    public void Subscribe(string topic, IActor consumer)
    {
        if (!_readers.ContainsKey(topic))
            _readers = _readers.Add(topic, new List<IActor>());
        var lst = _readers[topic];
        lst.Add(consumer);
        _readers = _readers.SetItem(topic, lst);
    }

    public void Unsubscribe(string topic, IActor consumer)
    {
        if (!_readers.ContainsKey(topic))
            return;
        _readers[topic].Remove(consumer);
    }

    public async Task Publish(string topic, IMsg msg, CancellationToken cancellationToken = default)
    {
            var consumers = _readers
                .Where(slot => slot.Key == topic)
                .SelectMany(slot => slot.Value);
            if (!consumers.Any()) return;
            foreach (var consumer in consumers)
            {
                await consumer.HandleCast(msg, cancellationToken);
            }
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
        _readers.Clear();
        Log.Information($"[{GetType().Name}] ~> Stopped.");
    }
}