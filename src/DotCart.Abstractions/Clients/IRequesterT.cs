using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Clients;

public interface IRequesterT<TPayload> : IDisposable
    where TPayload : IPayload
{
    Task<Feedback> RequestAsync(HopeT<TPayload> hope, CancellationToken cancellationToken = default);
}