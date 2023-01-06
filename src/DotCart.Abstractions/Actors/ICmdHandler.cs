using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public interface ICmdHandlerStep<TPipeInfo,TPayload> : IStepT<TPipeInfo,TPayload>
    where TPayload : IPayload 
    where TPipeInfo : IPipeInfoB
{
}

public interface ICmdHandler
{
    Task<Feedback> HandleAsync(ICmdB cmd, Feedback previous, CancellationToken cancellationToken = default);
}