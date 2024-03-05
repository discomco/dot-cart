using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.TestFirst.Drivers;

public delegate IEnumerable<IEvtB> EventStreamGenFunc<in TID>(TID ID)
    where TID : IID;