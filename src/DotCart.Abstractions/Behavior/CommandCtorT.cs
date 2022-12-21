using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public delegate Command CommandCtorT<out TCmd, in TPayload, in TMeta>(IID ID, TPayload payload, TMeta meta)
    where TCmd : ICmdT<TPayload>
    where TPayload : IPayload
    where TMeta : IEventMeta;