using DotCart.Abstractions.Behavior;

namespace DotCart.Abstractions.Schema;

/// <summary>
///     Evt2DocVal functions are meant to check the validity of the projection
///     take the old and new TDocs as input, together with the applied event
///     and offer the opportunity to validate the projection.
/// </summary>
/// <typeparam name="TDoc">Type of the document the Event is projected onto.</typeparam>
/// <typeparam name="TIEvt">The IEvt Injection Discriminator (Injector) that uniquely identifies the Event</typeparam>
public delegate bool Evt2DocValidator<in TDoc, TPayload, TMeta>(TDoc input, TDoc output, IEvtB evt)
    where TDoc : IState
    where TMeta : IMetaB
    where TPayload : IPayload;