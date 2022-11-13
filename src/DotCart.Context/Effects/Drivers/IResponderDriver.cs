using DotCart.Contract.Dtos;

namespace DotCart.Context.Effects.Drivers;

public interface IResponderDriver<THope> : IDriver
    where THope : IHope
{
    Task StartRespondingAsync(CancellationToken cancellationToken);
    Task StopRespondingAsync(CancellationToken cancellationToken);
}