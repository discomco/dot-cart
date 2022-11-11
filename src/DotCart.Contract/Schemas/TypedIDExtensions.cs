using System.Text;

namespace DotCart.Contract.Schemas;

public static class TypedIDExtensions
{
    public static byte[] GetBytes(this IID iid)
    {
        return Encoding.UTF8.GetBytes(iid.Value);
    }
}