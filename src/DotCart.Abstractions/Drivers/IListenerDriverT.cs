using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IListenerDriver : IDriver
{
    Task StartListeningAsync(CancellationToken cancellationToken = default);
    Task StopListeningAsync(CancellationToken cancellationToken = default);
}


public interface IListenerDriverT<TFact>: IListenerDriver 
    where TFact: IFact
{
    Task<TFact> CreateFactAsync(object source, CancellationToken cancellationToken = default);
}