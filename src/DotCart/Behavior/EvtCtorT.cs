using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Behavior;

public delegate Event EvtCtorT<in TPayload, in TMeta>(IID ID, byte[] payload, byte[] meta)
    where TPayload : IPayload
    where TMeta : IMetaB;