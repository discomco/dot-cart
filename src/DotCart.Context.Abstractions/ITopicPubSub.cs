using DotCart.Contract.Dtos;

namespace DotCart.Context.Abstractions;

public interface ITopicPubSub<TMsg> where TMsg : IMsg
{
    void Subscribe(string topic, Action<TMsg> handler);
    Task SubscribeAsync(string topic, Func<TMsg, Task> handler, CancellationToken cancellationToken = default);
    Task PublishAsync(string topic, TMsg msg, CancellationToken cancellationToken = default);
    Task UnsubscribeAsync(string topic, Func<IEvt, Task> handler, CancellationToken cancellationToken = default);
}