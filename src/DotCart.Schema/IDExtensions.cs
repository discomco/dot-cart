using System.Text;

namespace DotCart.Schema;

public static class IdentityExtensions
{
    public static byte[] GetBytes(this IID iid)
    {
        return Encoding.UTF8.GetBytes(iid.Value);
    }
}