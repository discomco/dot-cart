using DotCart.Contract.Schemas;
using DotCart.Core;

namespace DotCart.TestKit.Schema;





[IDPrefix(TestConstants.TheIDPrefix)]
public record TheID : ID
{
    public TheID(string value = "") : base(TestConstants.TheIDPrefix, value)
    {
    }

    public static NewID<TheID> Ctor => () => New;

    public static TheID New => new(GuidUtils.LowerCaseGuid);
}