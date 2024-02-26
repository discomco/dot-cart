using DotCart.Abstractions.Behavior;

namespace DotCart.Abstractions.Schema;

public delegate TDoc Evt2Doc<TDoc, TPayload, TMeta>(TDoc doc, IEvtB evt)
    where TDoc : IState
    where TPayload : IPayload
    where TMeta : IMetaB;