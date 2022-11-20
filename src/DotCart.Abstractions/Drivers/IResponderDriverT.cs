using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IResponderDriverT<THope> : IDriver
    where THope : IHope
{
    Task StartRespondingAsync(CancellationToken cancellationToken = default);
    Task StopRespondingAsync(CancellationToken cancellationToken = default);
}