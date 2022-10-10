using DotCart.Contract;
using DotCart.Schema;

namespace DotCart.Domain;

public delegate bool SpecFunc<in TState, TCmd>(TState state) where TState : IState where TCmd:ICmd;

public interface IExec
{
}

public interface IExec<TState, in TCmd> : IExec
    where TState : IState
    where TCmd : ICmd
{
    IFeedback Verify(TState state,  TCmd cmd);
    IEnumerable<IEvt> Exec(TState state, TCmd cmd);
}

