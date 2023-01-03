using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Clients;

public abstract class RequesterT<TPayload> : IRequesterT<TPayload> where TPayload : IPayload
{
    public abstract Task<Feedback> RequestAsync(HopeT<TPayload> hope, CancellationToken cancellationToken = default);
    public abstract void Dispose();
}