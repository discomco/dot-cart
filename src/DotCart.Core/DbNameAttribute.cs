using Ardalis.GuardClauses;

namespace DotCart.Core;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class DbNameAttribute : Attribute
{
    public DbNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}

public static class DbNameAtt
{
    public static string Get<T>()
    {
        var atts = (DbNameAttribute[])typeof(T).GetCustomAttributes(typeof(DbNameAttribute), true);
        if (atts.Length == 0)
            Guard.Against.AttributeNotDefined("DbName", atts, typeof(T).FullName);
        return atts[0].Name;
    }

    public static string Get(object obj)
    {
        var atts = (DbNameAttribute[])obj.GetType().GetCustomAttributes(typeof(DbNameAttribute), true);
        if (atts.Length == 0)
            Guard.Against.AttributeNotDefined("DbName", atts, obj.GetType().FullName);
        return atts[0].Name;
    }
}