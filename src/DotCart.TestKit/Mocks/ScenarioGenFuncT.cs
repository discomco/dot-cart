using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.TestKit.Mocks;

public delegate IEnumerable<ICmdB> ScenarioGenFuncT<in TID>(TID ID)
    where TID : IID;