using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Clients;

public interface IRequesterT<TPayload> : IDisposable
    where TPayload : IPayload
{
    Task<IFeedback> RequestAsync(
        IHopeT<TPayload> hope,
        CancellationToken cancellationToken = default);
}