using System.Text;

namespace DotCart.Client.Schemas;

public static class TypedIDExtensions
{
    public static byte[] GetBytes(this IID iid)
    {
        return Encoding.UTF8.GetBytes(iid.Value);
    }
}