using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public delegate Command Hope2Cmd<TPayload, TMeta>(HopeT<TPayload> hope)
    where TPayload : IPayload
    where TMeta : IMetaB;