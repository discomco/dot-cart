using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public interface IApply
{
    string EvtType { get; }
    IApply SetAggregate(IAggregate aggregate);
}

public interface IApply<TState, TPayload, TMeta> : IApply
    where TState : IState
    where TPayload : IPayload
    where TMeta : IMetaB
{
    TState Apply(TState state, IEvtB evt);
}