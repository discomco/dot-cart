using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public interface ICmdHandlerStep<TPipeInfo, TPayload>
    : IStepT<TPipeInfo, TPayload>
    where TPayload : IPayload
    where TPipeInfo : IPipeInfoB;

public interface ICmdHandler
{
    Task<IFeedback> HandleAsync(
        ICmdB cmd,
        IFeedback previous,
        CancellationToken cancellationToken = default);
}