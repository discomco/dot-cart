using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using Serilog;
using static System.Threading.Tasks.Task;

namespace DotCart.Actors;

internal abstract class TopicPubSub<TMsg> : ITopicPubSub<TMsg> where TMsg : IMsg
{
    private static readonly AsyncLocal<Dictionary<string, List<object>>> handlers =
        new();

    private readonly object _subMutex = new();

    private object _pubMutex = new();

    public Dictionary<string, List<object>> Handlers =>
        handlers.Value
        ?? (handlers.Value = new Dictionary<string, List<object>>());

    public void Subscribe(string topic, Action<TMsg> handler)
    {
        GetHandlersOf(topic)
            .Add(handler);
    }

    public Task SubscribeAsync(string topic, Func<TMsg, Task> handler, CancellationToken cancellationToken)
    {
        return Run(() => { GetHandlersOf(topic).Add(handler); }, cancellationToken);
    }

    public Task PublishAsync(string topic, TMsg msg, CancellationToken cancellationToken = default)
    {
        return Run(async () =>
        {
            foreach (var handler in GetHandlersOf(topic))
                try
                {
                    switch (handler)
                    {
                        case Action<TMsg> action:
                            action(msg);
                            break;
                        case Func<TMsg, Task> action:
                            await action(msg);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Log.Fatal(e.Message);
                    throw;
                }
        }, cancellationToken);
    }

    public Task UnsubscribeAsync(string topic, Func<IEvtB, Task> handler, CancellationToken cancellationToken = default)
    {
        return Run(() => { GetHandlersOf(topic).Remove(handler); }, cancellationToken);
    }

    public void Dispose()
    {
        foreach (var handlersOfTopic in Handlers.Values) handlersOfTopic.Clear();
        Handlers.Clear();
    }

    private ICollection<object> GetHandlersOf(string topic)
    {
        var handlers =
            Handlers.GetValueOrDefault(topic)
            ?? (Handlers[topic] = new List<object>());
        return handlers;
    }
}