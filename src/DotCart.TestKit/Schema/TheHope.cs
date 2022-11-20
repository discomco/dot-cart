using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.TestKit.Schema;

[Topic(TestConstants.TheHopeTopic)]
public record TheHope(string AggId, byte[] Data) : Dto(AggId, Data), IHope<ThePayload>
{
    public static TheHope New(string aggId, byte[] data)
    {
        return new TheHope(aggId, data);
    }

    public static TheHope New(string aggId, ThePayload payload)
    {
        return new TheHope(aggId, payload.ToBytes());
    }
}