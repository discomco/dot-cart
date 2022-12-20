using DotCart.Abstractions.Schema;
using DotCart.TestKit.Schema;

namespace DotCart.TestKit;

public static class Utils
{
    public static IDCtorT<TheID>
        IDCtor =
            value => TheID.New;
}