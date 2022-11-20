using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public interface IExchange : IActor
{
    void Subscribe(string topic, IActor consumer);
    void Unsubscribe(string topic, IActor consumer);
    Task Publish(string topic, IMsg msg, CancellationToken cancellationToken = default);
}