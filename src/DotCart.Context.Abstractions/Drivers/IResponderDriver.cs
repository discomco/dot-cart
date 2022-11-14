using DotCart.Contract.Dtos;

namespace DotCart.Context.Abstractions.Drivers;

public interface IResponderDriver<THope> : IDriver
    where THope : IHope
{
    Task StartRespondingAsync(CancellationToken cancellationToken);
    Task StopRespondingAsync(CancellationToken cancellationToken);
}