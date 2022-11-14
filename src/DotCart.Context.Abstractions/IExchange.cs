using DotCart.Contract.Dtos;

namespace DotCart.Context.Abstractions;

public interface IExchange : IActor
{
    void Subscribe(string topic, IActor consumer);
    void Unsubscribe(string topic, IActor consumer);
    Task Publish(string topic, IMsg msg, CancellationToken cancellationToken = default);
}