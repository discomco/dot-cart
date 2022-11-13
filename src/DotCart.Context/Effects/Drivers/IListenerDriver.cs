namespace DotCart.Context.Effects.Drivers;

public interface IListenerDriver : IDriver
{
    Task StartListening<TFact>(CancellationToken cancellationToken);
    Task StopListening<TFact>(CancellationToken cancellationToken);
}