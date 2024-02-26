using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;

namespace DotCart.Schema;

public delegate FactT<TPayload, TMeta> Msg2Fact<TPayload, TMeta, in TDriverMsg>(TDriverMsg msg)
    where TPayload : IPayload
    where TMeta : IMetaB
    where TDriverMsg : class;