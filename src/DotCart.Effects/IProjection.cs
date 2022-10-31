using DotCart.Schema;

namespace DotCart.Effects;

public interface IProjection<TState, in TEvt> : IEffect
    where TState : IState
{
}