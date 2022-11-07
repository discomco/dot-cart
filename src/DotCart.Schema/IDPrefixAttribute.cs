namespace DotCart.Schema;

[AttributeUsage(AttributeTargets.Class)]
public class IDPrefixAttribute : Attribute
{
    public IDPrefixAttribute(string prefix)
    {
        Prefix = prefix;
    }

    public string Prefix { get; set; }
}

public static class IDPrefix
{
    public static string Get<TID>()
        where TID : IID
    {
        var prefixAttributes =
            (IDPrefixAttribute[])typeof(TID).GetCustomAttributes(typeof(IDPrefixAttribute), true);
        if (prefixAttributes.Length <= 0)
            throw new IDPrefixNotSetException($"[IDPrefix] attribute is not set for {typeof(TID)} ");
        var att = prefixAttributes[0];
        return att.Prefix;
    }
}