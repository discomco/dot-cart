using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IEmitterDriverB : IDriver
{
}

public interface IEmitterDriverT<TPayload, TDriverMsg> : IEmitterDriverB
    where TDriverMsg : class 
    where TPayload : IPayload
{
    Task EmitAsync(FactT<TPayload> fact, CancellationToken cancellationToken = default);
}