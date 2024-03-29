using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public class ApplyEvtT<TState, TPayload, TMeta>
    : IApply<TState, TPayload, TMeta>
    where TState : IState
    where TPayload : IPayload
    where TMeta : IMetaB
{
    private readonly Evt2Doc<TState, TPayload, TMeta> _evt2State;

    protected IAggregate Aggregate;

    public ApplyEvtT(Evt2Doc<TState, TPayload, TMeta> evt2State)
    {
        _evt2State = evt2State;
    }

    public IApply SetAggregate(IAggregate aggregate)
    {
        Aggregate = aggregate;
        return this;
    }

    public string EvtType => EvtTopicAtt.Get<TPayload>();

    public TState Apply(TState state, IEvtB evt)
    {
        return _evt2State(state, evt);
    }
}