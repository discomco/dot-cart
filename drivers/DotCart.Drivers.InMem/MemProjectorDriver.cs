using DotCart.Context.Abstractions;
using DotCart.Context.Abstractions.Drivers;

namespace DotCart.Drivers.InMem;

public class MemProjectorDriver : IProjectorDriver
{
    private IActor _projector;

    public MemProjectorDriver(IExchange exchange)
    {
    }


    public void Dispose()
    {
    }

    public Task StartStreamingAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopStreamingAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void SetActor(IActor actor)
    {
        _projector = actor;
    }
}