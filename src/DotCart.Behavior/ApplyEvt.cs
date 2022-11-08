using DotCart.Schema;

namespace DotCart.Behavior;


public abstract class ApplyEvt<TState, TEvt> : IApply 
    where TEvt: IEvt 
    where TState: IState
{
    protected IAggregate Aggregate;
    public abstract TState Apply(TState state, Event evt);
    public void SetAggregate(IAggregate aggregate)
    {
        Aggregate = aggregate;
    }
    public string EvtType => Topic.Get<TEvt>();
}