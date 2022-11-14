using DotCart.Contract.Schemas;
using DotCart.Core;

namespace DotCart.TestKit;

[IDPrefix("my")]
public record MyID : ID
{
    public MyID(string value = "") : base("my", value)
    {
    }

    public static NewID<MyID> Ctor => () => New;

    public static MyID New => new(GuidUtils.LowerCaseGuid);
}