using DotCart.Abstractions.Schema;


namespace DotCart.Abstractions.Actors;

[Name(AttributeConstants.ExchangeName)]
public interface IExchange
    : IActorB
{
    void Subscribe(string topic, IActorB consumer);
    void Unsubscribe(string topic, IActorB consumer);
    Task Publish(string topic, IMsg msg, CancellationToken cancellationToken = default);
}