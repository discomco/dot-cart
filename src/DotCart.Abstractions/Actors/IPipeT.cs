using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public interface IPipeT<TPipeInfo, TPayload>
    where TPayload : IPayload
    where TPipeInfo : IPipeInfoB
{
    string Name { get; }
    int StepCount { get; }
    Task<IFeedback> ExecuteAsync(IDto msg,
        CancellationToken cancellationToken = default);
}