using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Clients;

public abstract class RequesterT<TPayload>
    : IRequesterT<TPayload>
    where TPayload : IPayload
{
    public abstract Task<IFeedback> RequestAsync(IHopeT<TPayload> hope, CancellationToken cancellationToken = default);
    public abstract void Dispose();
}