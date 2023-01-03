using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IListenerDriverB : IDriverB
{
    Task StartListeningAsync(CancellationToken cancellationToken = default);
    Task StopListeningAsync(CancellationToken cancellationToken = default);
    string Topic { get; }
}

public interface IListenerDriverT<TFactPayload, TDriverMsg> : IListenerDriverB
    where TFactPayload : IPayload
    where TDriverMsg : class
{
}