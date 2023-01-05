using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;

namespace DotCart.Defaults.RabbitMq;

public interface IRmqEmitterDriverT<TPayload, TMeta> : IEmitterDriverT<TPayload, TMeta>
    where TPayload : IPayload
    where TMeta : IEventMeta
{
}