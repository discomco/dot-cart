using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.TestKit.Schema;

[Topic("dotcart:the_msg")]
public record TheMsg : IMsg
{
    public static TheMsg Random => new();
}