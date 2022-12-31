using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IListenerDriverB : IDriverB
{
    Task StartListeningAsync(CancellationToken cancellationToken = default);
    Task StopListeningAsync(CancellationToken cancellationToken = default);
}

public interface IListenerDriverT<TIFact, TDriverMsg> : IListenerDriverB
    where TDriverMsg : class
    where TIFact : IFactB

{
}