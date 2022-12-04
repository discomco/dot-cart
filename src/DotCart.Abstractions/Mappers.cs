using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions;

public delegate TCmd Evt2Cmd<out TCmd, in TEvt>(TEvt Evt)
    where TEvt : IEvt
    where TCmd : ICmd;

public delegate TCmd Fact2Cmd<out TCmd, in TFact>(TFact fact)
    where TFact : IFact
    where TCmd : ICmd;

public delegate TState Evt2State<TState, in TEvt>(TState state, TEvt evt)
    where TState : IState
    where TEvt : IEvt;

public delegate TFact Evt2Fact<out TFact, in TEvt>(TEvt evt)
    where TFact : IFact
    where TEvt : IEvt;

