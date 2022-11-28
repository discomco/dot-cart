using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit.Schema;

namespace DotCart.TestKit.Behavior;

[Topic(TestConstants.CmdTopic)]
public record TheCmd(ID AggregateID, ThePayload Payload)
    : CmdT<ThePayload>(AggregateID, Payload)
{
    public static TheCmd New(string aggId, byte[] data)
    {
        return new TheCmd(
            aggId.IDFromIdString(),
            data.FromBytes<ThePayload>());
    }
}