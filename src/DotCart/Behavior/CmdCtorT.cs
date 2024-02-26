using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Behavior;

public delegate
    Command CmdCtorT<in TID, in TPayload, in TMeta>(TID ID, TPayload payload, TMeta meta)
    where TID : IID
    where TPayload : IPayload
    where TMeta : IMetaB;