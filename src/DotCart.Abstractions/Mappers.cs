using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions;

public delegate TCmd Evt2Cmd<out TCmd, in TEvt>(Event Evt, IState state)
    where TEvt : IEvtB
    where TCmd : ICmdB;

public delegate TCmd Fact2Cmd<out TCmd, in TFact>(TFact fact)
    where TFact : IFactB
    where TCmd : ICmdB;

/// <summary>
/// Evt2DocVal functions are meant to check the validity of the projection
/// take the old and new TDocs as input, together with the applied event
/// and offer the opportunity to validate the projection. 
/// </summary>
/// <typeparam name="TDoc">Type of the document the Event is projected onto.</typeparam>
/// <typeparam name="TIEvt">The IEvt Injection Discriminator (Injector) that uniquely identifies the Event</typeparam>
public delegate bool Evt2DocValidator<in TDoc, in TIEvt>(TDoc input, TDoc output, Event evt) 
    where TDoc : IState 
    where TIEvt : IEvtB;


public delegate TDoc Evt2Doc<TDoc, in TEvt>(TDoc doc, Event evt)
    where TDoc : IState
    where TEvt : IEvtB;

public delegate TFact Evt2Fact<out TFact, in TEvt>(Event evt)
    where TFact : IFactB
    where TEvt : IEvtB;