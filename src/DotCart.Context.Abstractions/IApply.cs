using DotCart.Contract.Schemas;

namespace DotCart.Context.Abstractions;

public interface IApply
{
    string EvtType { get; }
    void SetAggregate(IAggregate aggregate);
}

public interface IApply<in TState, in TEvt> : IApply
    where TEvt : IEvt
    where TState : IState
{
    TState Apply<TState>(TState state, Event evt);
}