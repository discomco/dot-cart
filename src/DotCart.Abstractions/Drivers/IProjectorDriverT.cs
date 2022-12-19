using DotCart.Abstractions.Behavior;

namespace DotCart.Abstractions.Drivers;

public interface IProjectorDriver : IDriver
{
    Task StartStreamingAsync(CancellationToken cancellationToken = default);
    Task StopStreamingAsync(CancellationToken cancellationToken = default);
}

public interface IProjectorDriverT<TInfo> : IProjectorDriver
    where TInfo : ISubscriptionInfo
{
    Task<IEvtB> CreateEventAsync(object source, CancellationToken cancellationToken = default);
}