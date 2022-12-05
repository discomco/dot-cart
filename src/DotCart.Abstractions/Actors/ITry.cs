using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public interface ITry
{
    string CmdType { get; }
    void SetAggregate(IAggregate aggregate);
}

// public interface ITry<in TCmd> : ITry
//     where TCmd : ICmd
// {
// }

public interface ITry<in TCmd, in TState> : ITry
    where TCmd : ICmd
    where TState : IState
{
    IFeedback Verify(TCmd cmd, TState state);
    IEnumerable<IEvt> Raise(TCmd cmd, TState state);
}
