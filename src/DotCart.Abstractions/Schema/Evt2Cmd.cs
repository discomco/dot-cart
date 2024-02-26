using DotCart.Abstractions.Behavior;

namespace DotCart.Abstractions.Schema;

public delegate Command Evt2Cmd<TCmdPayload, TEvtPayload, TMeta>(Event Evt, IState state)
    where TCmdPayload : IPayload
    where TMeta : IMetaB;