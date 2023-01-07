using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IEmitterDriverB : IDriverB
{
    Task ConnectAsync();
}

public interface IEmitterDriverT<TPayload, TMeta> : IEmitterDriverB
    where TPayload : IPayload
    where TMeta : IMeta
{
    Task EmitAsync(FactT<TPayload, TMeta> fact, CancellationToken cancellationToken = default);
}