using DotCart.Schema;

namespace DotCart.Behavior;

public interface IApply
{
    void SetAggregate(IAggregate aggregate);
    string EvtType { get;  }
}

public interface IApply<in TState, in TEvt> : IApply
    where TEvt : IEvt
    where TState : IState
{
    TState Apply<TState>(TState state, Event evt);
}