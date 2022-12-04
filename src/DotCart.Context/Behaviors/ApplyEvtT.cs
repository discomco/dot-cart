using DotCart.Abstractions;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.Context.Behaviors;

public abstract class ApplyEvtT<TState, TEvt> : IApply
    where TEvt : IEvt
    where TState : IState
{
    private readonly Evt2State<TState, TEvt> _evt2State;

    public ApplyEvtT(Evt2State<TState, TEvt> evt2State)
    {
        _evt2State = evt2State;
    }
    
    protected IAggregate Aggregate;

    public void SetAggregate(IAggregate aggregate)
    {
        Aggregate = aggregate;
    }

    public string EvtType => TopicAtt.Get<TEvt>();

    public TState Apply(TState state, TEvt evt)
    {
        return _evt2State(state, evt);
    }
}