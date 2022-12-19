using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit.Contract;
using DotCart.TestKit.Schema;

namespace DotCart.TestKit.Behavior;

[Topic("the:event")]
public interface ITheEvt : IEvtT<ThePayload> {}

//
// public record TheEvt(IID AggregateID, ThePayload Payload, TheMeta Meta)
//     : EvtT<ThePayload, TheMeta>(AggregateID, TopicAtt.Get<TheEvt>(), Payload, Meta); 