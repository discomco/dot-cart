using DotCart.Contract.Dtos;
using DotCart.Core;

namespace DotCart.TestKit;

[Topic("dotcart:the_msg")]
public record TheMsg() : Msg(TopicAtt.Get<TheMsg>(), TheDoc.Rand.ToBytes())
{
    public static TheMsg Random => new();
}