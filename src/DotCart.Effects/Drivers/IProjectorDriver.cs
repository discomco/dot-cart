namespace DotCart.Effects.Drivers;

public interface IProjectorDriver : IDriver
{
    Task StartStreamingAsync(CancellationToken cancellationToken);
    Task StopStreamingAsync(CancellationToken cancellationToken);
}