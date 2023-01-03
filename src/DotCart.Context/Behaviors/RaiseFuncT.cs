using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Context.Behaviors;

public delegate IEnumerable<IEvtB> RaiseFuncT<in TState, TPayload, TMeta>(Command cmd, TState state)
    where TState : IState
    where TPayload : IPayload
    where TMeta : IEventMeta;