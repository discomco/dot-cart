using DotCart.Schema;

namespace DotCart.Behavior;

public interface IProjection<TState, in TEvt> : IEffect 
    where TState : IState
{
}