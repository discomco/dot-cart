using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public interface IApply
{
    string EvtType { get; }
    IApply SetAggregate(IAggregate aggregate);
}


public interface IApply<TState, TPayload, TMeta> : IApply
    where TState : IState
{
    TState Apply(TState state, EvtT<TPayload,TMeta> evt);
}