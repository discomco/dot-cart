namespace DotCart.Context.Abstractions.Drivers;

public interface IProjectorDriver : IDriver
{
    Task StartStreamingAsync(CancellationToken cancellationToken);
    Task StopStreamingAsync(CancellationToken cancellationToken);
}

public interface IProjectorDriver<TInfo> : IProjectorDriver where TInfo : ISubscriptionInfo
{
}