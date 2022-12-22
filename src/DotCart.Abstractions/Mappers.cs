using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions;

public delegate TCmd Evt2Cmd<out TCmd, in TEvt>(Event Evt, IState state)
    where TEvt : IEvtB
    where TCmd : ICmdB;

public delegate TCmd Fact2Cmd<out TCmd, in TFact>(TFact fact)
    where TFact : IFactB
    where TCmd : ICmdB;

public delegate TState Evt2State<TState, in TEvt>(TState state, Event evt)
    where TState : IState
    where TEvt : IEvtB;

public delegate TFact Evt2Fact<out TFact, in TEvt>(Event evt)
    where TFact : IFactB
    where TEvt : IEvtB;