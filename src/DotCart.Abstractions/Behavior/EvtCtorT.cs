using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public delegate TEvt EvtCtorT<
    out TEvt,
    in TAggID>(TAggID ID)
    where TEvt : IEvtB
    where TAggID : IID;
