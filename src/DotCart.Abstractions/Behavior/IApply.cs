using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public interface IApply
{
    string EvtType { get; }
    IApply SetAggregate(IAggregate aggregate);
}

public interface IApply<TState, in TEvt> : IApply
    where TEvt : IEvtB
    where TState : IState
{
    TState Apply(TState state, Event evt);
}