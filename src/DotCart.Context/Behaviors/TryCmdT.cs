using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.Context.Behaviors;

public delegate Feedback VerifyFunc<in TState, in TCmd, TPayload>(TState state, TCmd cmd)
    where TState : IState
    where TCmd : ICmd<TPayload>
    where TPayload : IPayload;

public abstract class TryCmdT<TCmd, TState> : ITry<TCmd, TState> 
    where TCmd : ICmd 
    where TState : IState
{
    protected IAggregate Aggregate;
    public string CmdType => TopicAtt.Get<TCmd>();
    public void SetAggregate(IAggregate aggregate)
    {
        Aggregate = aggregate;
    }
    public abstract IFeedback Verify(TCmd cmd, TState state);
    public abstract IEnumerable<Event> Raise(TCmd cmd, TState state);
}