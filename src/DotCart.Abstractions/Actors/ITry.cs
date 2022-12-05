using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public delegate bool SpecFunc<in TState, TCmd>(TState state) where TState : IState where TCmd : ICmd;

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
    IEnumerable<Event> Raise(TCmd cmd, TState state);
}