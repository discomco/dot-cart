using Ardalis.GuardClauses;

namespace DotCart.Abstractions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class NameAttribute : Attribute
{
    public NameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}

public static class NameAtt
{
    public static string Get<T>()
    {
        var atts = (NameAttribute[])typeof(T).GetCustomAttributes(typeof(NameAttribute),
            true);
        Guard.Against.AttributeNotDefined("Name", atts, typeof(T).FullName);
        return atts[0].Name;
    }

    public static string Get(object obj)
    {
        var atts = (NameAttribute[])obj.GetType().GetCustomAttributes(typeof(NameAttribute),
            true);
        Guard.Against.AttributeNotDefined("Name", atts, obj.GetType().FullName);
        return atts[0].Name;
    }
}