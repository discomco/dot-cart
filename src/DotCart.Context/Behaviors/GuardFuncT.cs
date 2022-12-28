using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Context.Behaviors;

public delegate IFeedback GuardFuncT<in TState, in TCmd>(TCmd cmd, TState state)
    where TState : IState
    where TCmd : ICmdB;