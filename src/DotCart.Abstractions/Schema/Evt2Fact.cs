using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;

namespace DotCart.Abstractions.Schema;

public delegate FactT<TPayload, TMeta> Evt2Fact<TPayload, TMeta>(IEvtB evt)
    where TPayload : IPayload
    where TMeta : IMetaB;