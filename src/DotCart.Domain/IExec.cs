using DotCart.Contract;
using DotCart.Schema;

namespace DotCart.Domain;

public delegate bool SpecFunc<in TState, TCmd>(TState state) where TState : IState where TCmd:ICmd;

public interface IExec
{
}

public interface IExec<in TCmd> : IExec
    where TCmd : ICmd
{
    IFeedback Verify(TCmd cmd);
    IEnumerable<IEvt> Exec(TCmd cmd);
}

