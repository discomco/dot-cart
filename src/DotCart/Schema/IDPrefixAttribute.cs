using Ardalis.GuardClauses;
using DotCart.Abstractions;

namespace DotCart.Schema;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class IDPrefixAttribute : Attribute
{
    public IDPrefixAttribute(string prefix)
    {
        Prefix = prefix;
    }

    public string Prefix { get; set; }
}

public static class IDPrefixAtt
{
    public static string Get<TID>()
    {
        var prefixAttributes =
            (IDPrefixAttribute[])typeof(TID).GetCustomAttributes(typeof(IDPrefixAttribute), true);
        Guard.Against.AttributeNotDefined("IDPrefix", prefixAttributes, $"{typeof(TID)}");
        var att = prefixAttributes[0];
        return att.Prefix;
    }

    public static string Get(object obj)
    {
        var prefixAttributes =
            (IDPrefixAttribute[])obj.GetType().GetCustomAttributes(typeof(IDPrefixAttribute), true);
        Guard.Against.AttributeNotDefined("IDPrefix", prefixAttributes, $"{obj.GetType()}");
        var att = prefixAttributes[0];
        return att.Prefix;
    }
}