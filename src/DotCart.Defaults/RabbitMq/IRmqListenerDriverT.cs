using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;

namespace DotCart.Defaults.RabbitMq;

public interface IRmqListenerDriverT<TFactPayload>
    : IListenerDriverT<TFactPayload, byte[]>
    where TFactPayload : IPayload
{
}