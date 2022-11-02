using DotCart.Contract;

namespace DotCart.Effects.Drivers;

public interface IResponderDriver<THope>: IDriver 
    where THope: IHope
{
    Task StartRespondingAsync(CancellationToken cancellationToken);
    Task StopRespondingAsync(CancellationToken cancellationToken);

}