using DotCart.Abstractions;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.Context.Behaviors;

public class ApplyEvtT<TState, TEvt> : IApply<TState, TEvt>
    where TEvt : IEvtB
    where TState : IState
{
    private readonly Evt2State<TState, TEvt> _evt2State;

    protected IAggregate Aggregate;

    public ApplyEvtT(Evt2State<TState, TEvt> evt2State)
    {
        _evt2State = evt2State;
    }

    public void SetAggregate(IAggregate aggregate)
    {
        Aggregate = aggregate;
    }

    public string EvtType => TopicAtt.Get<TEvt>();

    public TState Apply(TState state, Event evt)
    {
        return _evt2State(state, evt);
    }
}