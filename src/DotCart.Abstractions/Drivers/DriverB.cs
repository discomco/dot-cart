using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public abstract class DriverB : IDriver
{
    private IActor _actor;
    public virtual void Dispose()
    {
        throw new NotImplementedException();
    }

    public void SetActor(IActor actor)
    {
        _actor = actor;
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