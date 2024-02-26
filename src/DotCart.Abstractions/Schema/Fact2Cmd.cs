using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;

namespace DotCart.Abstractions.Schema;

public delegate Command Fact2Cmd<TCmdPayload, TMeta, TFactPayload>(FactT<TFactPayload, TMeta> fact)
    where TCmdPayload : IPayload
    where TMeta : IMetaB
    where TFactPayload : IPayload;