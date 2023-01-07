using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public interface IProjectionB : IActor
{
}

public interface IProjectionT<TDriver, TState, TPayload, TMeta> : IProjectionB
    where TDriver : IDocStore<TState>
    where TState : IState
    where TPayload : IPayload
    where TMeta : IMeta
{
}