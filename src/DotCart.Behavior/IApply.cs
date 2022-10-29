using DotCart.Schema;

namespace DotCart.Behavior;

public interface IApply
{
}

public interface IApply<in TState, in TEvt> : IApply
    where TEvt : IEvt
    where TState : IState
{
    IState Apply(TState state, TEvt evt);
}