using DotCart.Abstractions.Behavior;

namespace DotCart.Abstractions.Schema;

public delegate FactT<TPayload, TMeta> Msg2Fact<TPayload, TMeta, in TDriverMsg>(TDriverMsg msg)
    where TPayload : IPayload
    where TMeta : IMetaB
    where TDriverMsg : class;