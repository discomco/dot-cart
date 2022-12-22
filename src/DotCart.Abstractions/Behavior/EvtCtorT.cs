using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public delegate Event EvtCtorT<
    TIEvt,
    in TPayload,
    in TMeta>(IID ID, TPayload payload, TMeta meta)
    where TIEvt : IEvtT<TPayload>
    where TPayload : IPayload
    where TMeta : IEventMeta;

