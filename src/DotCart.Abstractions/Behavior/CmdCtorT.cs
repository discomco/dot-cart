using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public delegate
    CmdT<TPayload, TMeta> CmdCtorT<in TID, TPayload, TMeta>(TID ID, TPayload payload, TMeta meta)
    where TID : IID
    where TPayload : IPayload
    where TMeta : IEventMeta;