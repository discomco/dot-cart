using DotCart.Behavior;
using DotCart.Contract;

namespace DotCart.Effects.Drivers;

public interface IResponderDriver: IDriver
{
    Task StartRespondingAsync(CancellationToken cancellationToken);
    Task StopRespondingAsync(CancellationToken cancellationToken);

}