namespace DotCart.Behavior;

public interface ITopicPubSub: IDisposable
{
    void Subscribe(string topic, Action<IEvt> handler);

    void Subscribe(string topic, Func<IEvt,Task> handler);

    Task PublishAsync(string topic, IEvt publishedEvent);
}