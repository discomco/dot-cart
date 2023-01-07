using DotCart.Abstractions.Schema;
using DotCart.TestKit.Mocks;

namespace DotCart.TestKit;

public static class Utils
{
    public static IDCtorT<TheSchema.ID>
        IDCtor =
            value => TheSchema.ID.New;
}