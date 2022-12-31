using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.Abstractions.Actors;


[Name(AttributeConstants.ExchangeName)]
public interface IExchange : IActor
{
    void Subscribe(string topic, IActor consumer);
    void Unsubscribe(string topic, IActor consumer);
    Task Publish(string topic, IMsg msg, CancellationToken cancellationToken = default);
}