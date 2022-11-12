using Ardalis.GuardClauses;
using DotCart.Drivers.Ardalis;

namespace DotCart.Contract.Schemas;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
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
    {
        var prefixAttributes =
            (IDPrefixAttribute[])typeof(TID).GetCustomAttributes(typeof(IDPrefixAttribute), true);
        Guard.Against.AttributeNotDefined("IDPrefix", prefixAttributes, $"{typeof(TID)}");
        var att = prefixAttributes[0];
        return att.Prefix;
    }

    public static string Get(IID id)
    {
        var prefixAttributes =
            (IDPrefixAttribute[])id.GetType().GetCustomAttributes(typeof(IDPrefixAttribute), true);
        Guard.Against.AttributeNotDefined("IDPrefix", prefixAttributes, $"{id.GetType()}");
        var att = prefixAttributes[0];
        return att.Prefix;
    }
}