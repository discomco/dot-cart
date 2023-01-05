using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.Context.Behavior;

public class TryCmdT<TState, TPayload, TMeta> : ITry<TState, TPayload, TMeta>
    where TState : IState
    where TPayload : IPayload
    where TMeta : IEventMeta
{
    private readonly RaiseFuncT<TState, TPayload, TMeta> _raise;
    private readonly GuardFuncT<TState, TPayload, TMeta> _specify;

    protected IAggregate Aggregate;

    public TryCmdT(GuardFuncT<TState, TPayload, TMeta> specify, RaiseFuncT<TState, TPayload, TMeta> raise)
    {
        _specify = specify;
        _raise = raise;
    }

    public string CmdType => CmdTopicAtt.Get<TPayload>();

    public ITry SetAggregate(IAggregate aggregate)
    {
        Aggregate = aggregate;
        return this;
    }

    public IFeedback Verify(Command cmd, TState state)
    {
        return _specify(cmd, state);
    }

    public IEnumerable<IEvtB> Raise(Command cmd, TState state)
    {
        return _raise(cmd, state);
    }
}