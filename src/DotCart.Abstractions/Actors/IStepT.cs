using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.Abstractions.Actors;

public interface IStepT<TPipeInfo,TPayload>
    where TPayload : IPayload 
    where TPipeInfo : IPipeInfoB
{
    IPipeT<TPipeInfo,TPayload> Pipe { get; }
    string Name { get; }
    int Order { get; }
    Importance Level { get; }
    Task<Feedback> ExecuteAsync(IDto msg, Feedback? previousFeedback, CancellationToken cancellationToken = default);
    void SetPipe(IPipeT<TPipeInfo,TPayload> pipe);
}