using DotCart.Abstractions;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.Context.Behaviors;

public class ApplyEvtT<TState, TEvt> : IApply<TState, TEvt>
    where TEvt : IEvtB
    where TState : IState
{
    private readonly Evt2Doc<TState, TEvt> _evt2State;

    protected IAggregate Aggregate;

    public ApplyEvtT(Evt2Doc<TState, TEvt> evt2State)
    {
        _evt2State = evt2State;
    }

    public IApply SetAggregate(IAggregate aggregate)
    {
        Aggregate = aggregate;
        return this;
    }

    public string EvtType => TopicAtt.Get<TEvt>();

    public TState Apply(TState state, Event evt)
    {
        return _evt2State(state, evt);
    }
}