using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Clients;

public interface IRequesterT<in THope> : IDisposable
    where THope : IHopeB
{
    Task<Feedback> RequestAsync(THope hope, CancellationToken cancellationToken = default);
}