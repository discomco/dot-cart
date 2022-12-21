using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IEmitterDriver : IDriver
{
}

public interface IEmitterDriverT<in TFact, TTarget> : IEmitterDriver
    where TFact : IFactB
    where TTarget : class
{
    Task EmitAsync(TFact fact, CancellationToken cancellationToken = default);
    Task<TTarget> ToTarget(TFact fact, CancellationToken cancellationToken = default);
}