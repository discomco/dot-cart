using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.Context.Behaviors;

public class TryCmdT<TCmd, TState> : ITry<TCmd, TState> 
    where TCmd : ICmd 
    where TState : IState
{
    private readonly SpecFuncT<TState, TCmd> _specify;
    private readonly RaiseFuncT<TState, TCmd> _raise;
    protected IAggregate Aggregate;
    public string CmdType => TopicAtt.Get<TCmd>();
    public void SetAggregate(IAggregate aggregate)
    {
        Aggregate = aggregate;
    }

    public TryCmdT(SpecFuncT<TState, TCmd> specify, RaiseFuncT<TState, TCmd> raise)
    {
        _specify = specify;
        _raise = raise;
    }

    public IFeedback Verify(TCmd cmd, TState state)
    {
        return _specify(cmd, state);
    }

    public IEnumerable<IEvt> Raise(TCmd cmd, TState state)
    {
        return _raise(cmd, state);
    }
}