using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;

namespace DotCart.Behavior;

public delegate Command Hope2Cmd<TPayload, TMeta>(HopeT<TPayload> hope)
    where TPayload : IPayload
    where TMeta : IMetaB;