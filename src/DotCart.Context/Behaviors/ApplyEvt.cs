using DotCart.Contract.Schemas;
using DotCart.Core;

namespace DotCart.Context.Behaviors;

public abstract class ApplyEvt<TState, TEvt> : IApply
    where TEvt : IEvt
    where TState : IState
{
    protected IAggregate Aggregate;

    public void SetAggregate(IAggregate aggregate)
    {
        Aggregate = aggregate;
    }

    public string EvtType => Topic.Get<TEvt>();
    public abstract TState Apply(TState state, Event evt);
}