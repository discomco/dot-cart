using Ardalis.GuardClauses;
using DotCart.Drivers.Ardalis;

namespace DotCart.Core;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class DbNameAttribute : Attribute
{
    public DbNameAttribute(string dbName)
    {
        DbName = dbName;
    }

    public string DbName { get; set; }
}

public static class DbNameAtt
{
    public static string Get<T>()
    {
        var atts = (DbNameAttribute[])typeof(T).GetCustomAttributes(typeof(DbNameAttribute),
            true);
        Guard.Against.AttributeNotDefined("DbName", atts, typeof(T).FullName);
        return atts[0].DbName;
    }

    public static string Get(object obj)
    {
        var atts = (DbNameAttribute[])obj.GetType().GetCustomAttributes(typeof(DbNameAttribute),
            true);
        Guard.Against.AttributeNotDefined("DbName", atts, obj.GetType().FullName);
        return atts[0].DbName;
    }
}