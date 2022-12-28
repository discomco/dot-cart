using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.Context.Behaviors;

public class TryCmdT<TCmd, TState> : ITry<TCmd, TState>
    where TCmd : ICmdB
    where TState : IState
{
    private readonly RaiseFuncT<TState, TCmd> _raise;
    private readonly GuardFuncT<TState, TCmd> _specify;
    protected IAggregate Aggregate;

    public TryCmdT(GuardFuncT<TState, TCmd> specify, RaiseFuncT<TState, TCmd> raise)
    {
        _specify = specify;
        _raise = raise;
    }

    public string CmdType => TopicAtt.Get<TCmd>();

    public void SetAggregate(IAggregate aggregate)
    {
        Aggregate = aggregate;
    }

    public IFeedback Verify(TCmd cmd, TState state)
    {
        return _specify(cmd, state);
    }

    public IEnumerable<IEvtB> Raise(TCmd cmd, TState state)
    {
        return _raise(cmd, state);
    }
}