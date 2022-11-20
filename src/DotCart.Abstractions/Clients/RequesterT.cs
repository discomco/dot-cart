using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Clients;

public abstract class RequesterT<THope> : IRequesterT<THope> where THope : IHope
{
    public abstract Task<Feedback> RequestAsync(THope hope, CancellationToken cancellationToken = default);
    public abstract void Dispose();
}