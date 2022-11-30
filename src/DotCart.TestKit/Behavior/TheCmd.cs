using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit.Schema;

namespace DotCart.TestKit.Behavior;

[Topic(TestConstants.CmdTopic)]
public record TheCmd(IID AggregateID, ThePayload Payload)
    : CmdT<ThePayload>(AggregateID, Payload)
{
    public static TheCmd New(string aggId, ThePayload payload)
    {
        return new TheCmd(aggId.IDFromIdString(), payload);
    }
}