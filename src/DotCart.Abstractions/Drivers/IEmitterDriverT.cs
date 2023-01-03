using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IEmitterDriverB : IDriverB
{
}

public interface IEmitterDriverT<TPayload, TMeta> : IEmitterDriverB
    where TPayload : IPayload
    where TMeta: IEventMeta
{
    Task EmitAsync(FactT<TPayload,TMeta> fact, CancellationToken cancellationToken = default);
}