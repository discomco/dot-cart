using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public interface ICmdHandlerStep<TPayload> : IStepT<TPayload>
    where TPayload : IPayload
{
}

public interface ICmdHandler
{
    Task<Feedback> HandleAsync(ICmdB cmd, Feedback previous, CancellationToken cancellationToken = default);
}