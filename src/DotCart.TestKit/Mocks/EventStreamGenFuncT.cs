using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.TestKit.Mocks;

public delegate IEnumerable<IEvtB> EventStreamGenFuncT<in TID>(TID ID)
    where TID : IID;