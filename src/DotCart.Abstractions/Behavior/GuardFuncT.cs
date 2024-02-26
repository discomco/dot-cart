using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public delegate IFeedback GuardFuncT<in TState, TPayload, TMeta>(ICmdB cmd, TState state)
    where TState : IState
    where TPayload : IPayload
    where TMeta : IMetaB;