using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public abstract class DriverB
    : IDriverB
{
    private IActorB _actor;

    public void SetActor(IActorB actor)
    {
        _actor = actor;
    }

    public virtual void Dispose()
    {
    }

    protected Task Cast(IMsg msg, CancellationToken cancellationToken = default)
    {
        return _actor.HandleCast(msg, cancellationToken);
    }

    protected Task<IMsg> Call(IMsg msg, CancellationToken cancellationToken = default)
    {
        return _actor.HandleCall(msg, cancellationToken);
    }
}