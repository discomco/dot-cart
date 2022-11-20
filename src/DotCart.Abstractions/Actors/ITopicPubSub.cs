using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public interface ITopicPubSub<TMsg> where TMsg : IMsg
{
    void Subscribe(string topic, Action<TMsg> handler);
    Task SubscribeAsync(string topic, Func<TMsg, Task> handler, CancellationToken cancellationToken = default);
    Task PublishAsync(string topic, TMsg msg, CancellationToken cancellationToken = default);
    Task UnsubscribeAsync(string topic, Func<IEvt, Task> handler, CancellationToken cancellationToken = default);
}