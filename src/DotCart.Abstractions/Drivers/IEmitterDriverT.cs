using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IEmitterDriverB : IDriverB
{
}

public interface IEmitterDriverT<TPayload> : IEmitterDriverB
    where TPayload : IPayload
{
    Task EmitAsync(FactT<TPayload> fact, CancellationToken cancellationToken = default);
}