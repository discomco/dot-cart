using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Context.Behaviors;

public delegate IEnumerable<IEvtB> RaiseFuncT<in TState, TPayload, TMeta>(CmdT<TPayload, TMeta> cmd, TState state)
    where TState : IState
    where TPayload : IPayload
    where TMeta : IEventMeta;