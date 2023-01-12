using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions;

public delegate Command Evt2Cmd<TCmdPayload, TEvtPayload, TMeta>(Event Evt, IState state)
    where TCmdPayload : IPayload
    where TMeta : IMetaB;

public delegate Command Fact2Cmd<TCmdPayload, TMeta, TFactPayload>(FactT<TFactPayload, TMeta> fact)
    where TCmdPayload : IPayload
    where TMeta : IMetaB
    where TFactPayload : IPayload;

/// <summary>
///     Evt2DocVal functions are meant to check the validity of the projection
///     take the old and new TDocs as input, together with the applied event
///     and offer the opportunity to validate the projection.
/// </summary>
/// <typeparam name="TDoc">Type of the document the Event is projected onto.</typeparam>
/// <typeparam name="TIEvt">The IEvt Injection Discriminator (Injector) that uniquely identifies the Event</typeparam>
public delegate bool Evt2DocValidator<in TDoc, TPayload, TMeta>(TDoc input, TDoc output, Event evt)
    where TDoc : IState
    where TMeta : IMetaB
    where TPayload : IPayload;

public delegate TDoc Evt2Doc<TDoc, TPayload, TMeta>(TDoc doc, Event evt)
    where TDoc : IState
    where TPayload : IPayload
    where TMeta : IMetaB;

public delegate FactT<TPayload, TMeta> Evt2Fact<TPayload, TMeta>(Event evt)
    where TPayload : IPayload
    where TMeta : IMetaB;