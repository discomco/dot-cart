using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions;

public delegate TCmd Evt2Cmd<out TCmd, in TEvt>(Event Evt)
    where TEvt : IEvt
    where TCmd : ICmd;

public delegate TCmd Fact2Cmd<out TCmd, in TFact>(TFact fact)
    where TFact : IFact
    where TCmd : ICmd;

public delegate TState Evt2State<TState, TEvt>(TState state, Event evt)
    where TState : IState
    where TEvt : IEvt;

public delegate TFact Evt2Fact<out TFact, in TEvt>(Event evt)
    where TFact : IFact
    where TEvt : IEvt;

public static class Mappers
{
}