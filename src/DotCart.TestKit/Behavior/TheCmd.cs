using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit.Contract;
using DotCart.TestKit.Schema;

namespace DotCart.TestKit.Behavior;

[Topic(TestConstants.CmdTopic)]
public record TheCmd(IID AggregateID, ThePayload Payload, EventMeta Meta)
    : CmdT<ThePayload, EventMeta>(AggregateID, Payload, Meta)
{
    public static TheCmd New(string aggId, ThePayload payload, EventMeta meta)
    {
        return new TheCmd(aggId.IDFromIdString(), payload, meta);
    }
}