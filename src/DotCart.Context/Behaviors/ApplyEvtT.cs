using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.Context.Behaviors;

public abstract class ApplyEvtT<TState, TEvt> : IApply
    where TEvt : IEvt
    where TState : IState
{
    protected IAggregate Aggregate;

    public void SetAggregate(IAggregate aggregate)
    {
        Aggregate = aggregate;
    }

    public string EvtType => TopicAtt.Get<TEvt>();
    public abstract TState Apply(TState state, Event evt);
}