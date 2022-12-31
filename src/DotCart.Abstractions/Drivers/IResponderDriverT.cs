using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IResponderDriver : IDriverB
{
    Task StartRespondingAsync(CancellationToken cancellationToken = default);
    Task StopRespondingAsync(CancellationToken cancellationToken = default);
}

public interface IResponderDriverT<THope> : IResponderDriver
    where THope : IHopeB
{
}