using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;

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
    Task<IFeedback> DoStepAsync(IDto msg,
        IFeedback fbk,
        CancellationToken cancellationToken);
}