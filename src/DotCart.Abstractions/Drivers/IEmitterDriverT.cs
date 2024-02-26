using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IEmitterDriverB
    : IDriverB
{
    Task ConnectAsync(
        CancellationToken cancellationToken = default);
}

public interface IEmitterDriverT<TPayload, TMeta>
    : IEmitterDriverB
    where TPayload : IPayload
    where TMeta : IMetaB
{
    Task EmitAsync(
        FactT<TPayload, TMeta> fact,
        CancellationToken cancellationToken = default);
}