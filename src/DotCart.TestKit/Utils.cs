using DotCart.Abstractions.Schema;

namespace DotCart.TestKit;

public static class Utils
{
    public static IDCtorT<TheSchema.ID>
        IDCtor =
            value => TheSchema.ID.New;
}