using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IRequesterDriverB : IDriver
{
    Task<Feedback> RequestAsync<THope>(THope hope, CancellationToken cancellationToken = default) where THope : IHope;
}