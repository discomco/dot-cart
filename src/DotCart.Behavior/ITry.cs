using DotCart.Contract;
using DotCart.Schema;

namespace DotCart.Behavior;

public delegate bool SpecFunc<in TState, TCmd>(TState state) where TState : IState where TCmd:ICmd;

public interface ITry
{
}

public interface ITry<in TCmd> : ITry
    where TCmd : ICmd
{
    IFeedback Verify(TCmd cmd);
    IEnumerable<IEvt> Raise(TCmd cmd);
}

