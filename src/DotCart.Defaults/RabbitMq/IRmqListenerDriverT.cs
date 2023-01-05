using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;

namespace DotCart.Defaults.RabbitMq;

public interface IRmqListenerDriverT<TFactPayload, TFactMeta>
    : IListenerDriverT<TFactPayload, TFactMeta>
    where TFactPayload : IPayload
    where TFactMeta : class
{
}