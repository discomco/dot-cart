using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.Abstractions.Actors;

public interface IStepT<TPipeInfo, TPayload>
    where TPayload : IPayload
    where TPipeInfo : IPipeInfoB
{
    IPipeT<TPipeInfo, TPayload> Pipe { get; }
    string Name { get; }
    int Order { get; }
    Importance Level { get; }
    void SetPipe(IPipeT<TPipeInfo, TPayload> pipe);
    Task<Feedback> DoStepAsync(IDto msg, Feedback fbk, CancellationToken cancellationToken);
}