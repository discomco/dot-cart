using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Domain;


public static partial class Inject
{
    public static IServiceCollection AddTopicPubSub(this IServiceCollection services)
    {
        return services?
            .AddSingleton<ITopicPubSub, TopicPubSub>();
    } 
}


public interface ITopicPubSub: IDisposable
{
    void Subscribe(string topic, Action<IEvt> handler);

    void Subscribe(string topic, Func<IEvt,Task> handler);

    Task PublishAsync(string topic, IEvt publishedEvent);
}


public class TopicPubSub : ITopicPubSub
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

    public void Subscribe(string topic, Action<IEvt> handler)
    {
        GetHandlersOf(topic).Add(handler);
    }

    public void Subscribe(string topic, Func<IEvt,Task> handler)
    {
        GetHandlersOf(topic).Add(handler);
    }

    public async Task PublishAsync(string topic, IEvt publishedEvent)
    {
        foreach (var handler in GetHandlersOf(topic))
            try
            {
                switch (handler)
                {
                    case Action<IEvt> action:
                        action(publishedEvent);
                        break;
                    case Func<IEvt, Task> action:
                        await action(publishedEvent);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
    }
    
    private ICollection<object> GetHandlersOf(string topic)
    {
        return Handlers.GetValueOrDefault(topic) ?? (Handlers[topic] = new List<object>());
    }
    
}