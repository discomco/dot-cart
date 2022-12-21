using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.TestKit.Schema;

[Topic(TestConstants.TheHopeTopic)]
public record TheHope(string AggId, ThePayload Payload) : HopeT<ThePayload>(AggId, Payload), IHopeT<ThePayload>
{
    public static TheHope New(string aggId, ThePayload payload)
    {
        return new TheHope(aggId, payload);
    }
}