using System.Text;

namespace DotCart.Schema;

public static class IdentityExtensions
{
    public static byte[] GetBytes(this IIdentity identity)
    {
        return Encoding.UTF8.GetBytes(identity.Value);
    }
}