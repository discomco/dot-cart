using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IListenerDriverB : IDriverB
{
    string Topic { get; }
    Task StartListeningAsync(CancellationToken cancellationToken = default);
    Task StopListeningAsync(CancellationToken cancellationToken = default);
}

public interface IListenerDriverT<TFactPayload, TDriverMsg> : IListenerDriverB
    where TFactPayload : IPayload
    where TDriverMsg : class
{
}