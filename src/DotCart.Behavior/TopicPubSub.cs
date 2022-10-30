using DotCart.Contract;
using Serilog;

namespace DotCart.Behavior;

public interface ITopicPubSub<TMsg> where TMsg: IMsg
{
    void Subscribe(string topic, Action<TMsg> handler);
    void Subscribe(string topic, Func<TMsg, Task> handler);
    Task PublishAsync(string topic, TMsg msg);
}



public abstract class TopicPubSub<TMsg> : ITopicPubSub<TMsg> where TMsg:IMsg
{
    private static readonly AsyncLocal<Dictionary<string, List<object>>> handlers =
        new();

    public Dictionary<string, List<object>> Handlers =>
        handlers.Value ?? (handlers.Value = new Dictionary<string, List<object>>());

    public void Dispose()
    {
        foreach (var handlersOfTopic in Handlers.Values) handlersOfTopic.Clear();
        Handlers.Clear();
    }

    public void Subscribe(string topic, Action<TMsg> handler)
    {
        GetHandlersOf(topic).Add(handler);
    }

    public void Subscribe(string topic, Func<TMsg, Task> handler)
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

    private ICollection<object> GetHandlersOf(string topic)
    {
        return Handlers.GetValueOrDefault(topic) ?? (Handlers[topic] = new List<object>());
    }
}