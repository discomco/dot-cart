using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public interface ITry
{
    string CmdType { get; }
    ITry SetAggregate(IAggregate aggregate);
}

public interface ITry<in TCmd, in TState> : ITry
    where TCmd : ICmdB
    where TState : IState
{
    IFeedback Verify(TCmd cmd, TState state);
    IEnumerable<IEvtB> Raise(TCmd cmd, TState state);
}