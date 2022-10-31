using DotCart.Schema;

namespace DotCart.Effects;

public interface IProjection<TState, in TEvt> : IReactor
    where TState : IState
{
}