using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions;

public delegate CmdT<TCmdPayload, TMeta> Evt2Cmd<TCmdPayload, TEvtPayload, TMeta>(EvtT<TEvtPayload, TMeta> Evt,
    IState state)
    where TCmdPayload : IPayload
    where TMeta : IEventMeta;

public delegate CmdT<TCmdPayload, TMeta> Fact2Cmd<TCmdPayload, TMeta, TFactPayload>(FactT<TFactPayload, TMeta> fact)
    where TCmdPayload : IPayload
    where TMeta : IEventMeta
    where TFactPayload : IPayload;

/// <summary>
///     Evt2DocVal functions are meant to check the validity of the projection
///     take the old and new TDocs as input, together with the applied event
///     and offer the opportunity to validate the projection.
/// </summary>
/// <typeparam name="TDoc">Type of the document the Event is projected onto.</typeparam>
/// <typeparam name="TIEvt">The IEvt Injection Discriminator (Injector) that uniquely identifies the Event</typeparam>
public delegate bool Evt2DocValidator<in TDoc, TPayload, TMeta>(TDoc input, TDoc output, EvtT<TPayload, TMeta> evt)
    where TDoc : IState
    where TMeta : IEventMeta
    where TPayload : IPayload;

public delegate TDoc Evt2Doc<TDoc, TPayload, TMeta>(TDoc doc, EvtT<TPayload, TMeta> evt)
    where TDoc : IState
    where TPayload : IPayload
    where TMeta : IEventMeta;

public delegate FactT<TPayload, TMeta> Evt2Fact<TPayload, TMeta>(EvtT<TPayload, TMeta> evt)
    where TPayload : IPayload
    where TMeta : IEventMeta;