using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions;

public delegate TCmd Evt2Cmd<out TCmd, in TEvt>(Event Evt, IState state)
    where TEvt : IEvtB
    where TCmd : ICmdB;

public delegate TCmd Fact2Cmd<out TCmd, in TFact>(TFact fact)
    where TFact : IFactB
    where TCmd : ICmdB;

public delegate TDoc Evt2Doc<TDoc, in TEvt>(TDoc doc, Event evt)
    where TDoc : IState
    where TEvt : IEvtB;

public delegate TFact Evt2Fact<out TFact, in TEvt>(Event evt)
    where TFact : IFactB
    where TEvt : IEvtB;