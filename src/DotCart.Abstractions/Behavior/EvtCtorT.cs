using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public delegate EvtT<TPayload, TMeta> EvtCtorT<TPayload, TMeta>(IID ID, TPayload payload, TMeta meta)
    where TPayload : IPayload
    where TMeta : IEventMeta;