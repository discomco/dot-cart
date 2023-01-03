using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public interface ITry
{
    string CmdType { get; }
    ITry SetAggregate(IAggregate aggregate);
}

public interface ITry<in TState, TPayload, TMeta> : ITry
    where TState : IState
    where TPayload : IPayload
    where TMeta : IEventMeta
{
    IFeedback Verify(CmdT<TPayload, TMeta> cmd, TState state);
    IEnumerable<IEvtB> Raise(CmdT<TPayload, TMeta> cmd, TState state);
}