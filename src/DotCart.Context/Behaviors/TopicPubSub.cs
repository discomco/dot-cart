using DotCart.Contract.Dtos;
using Serilog;

namespace DotCart.Context.Behaviors;

public interface ITopicPubSub<TMsg> where TMsg : IMsg
{
    void Subscribe(string topic, Action<TMsg> handler);
    void SubscribeAsync(string topic, Func<TMsg, Task> handler);
    Task PublishAsync(string topic, TMsg msg);
    Task UnsubscribeAsync(string topic, Func<IEvt, Task> handler);
}

public abstract class TopicPubSub<TMsg> : ITopicPubSub<TMsg> where TMsg : IMsg
{
    private static readonly AsyncLocal<Dictionary<string, List<object>>> handlers =
        new();

    public Dictionary<string, List<object>> Handlers =>
        handlers.Value ?? (handlers.Value = new Dictionary<string, List<object>>());

    public void Subscribe(string topic, Action<TMsg> handler)
    {
        GetHandlersOf(topic).Add(handler);
    }

    public void SubscribeAsync(string topic, Func<TMsg, Task> handler)
    {
        GetHandlersOf(topic).Add(handler);
    }

    public async Task PublishAsync(string topic, TMsg msg)
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
    }

    public Task UnsubscribeAsync(string topic, Func<IEvt, Task> handler)
    {
        return Task.Run(() => { GetHandlersOf(topic).Remove(handler); });
    }

    public void Dispose()
    {
        foreach (var handlersOfTopic in Handlers.Values) handlersOfTopic.Clear();
        Handlers.Clear();
    }

    private ICollection<object> GetHandlersOf(string topic)
    {
        return Handlers.GetValueOrDefault(topic) ?? (Handlers[topic] = new List<object>());
    }
}