using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Drivers;

namespace DotCart.Actors;

public class MemProjectorDriver : IProjectorDriver
{
    private IActorB _projector;

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

    public void SetActor(IActorB actor)
    {
        _projector = actor;
    }
}