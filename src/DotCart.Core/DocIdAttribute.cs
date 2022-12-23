using Ardalis.GuardClauses;
using DotCart.Drivers.Ardalis;

namespace DotCart.Core;

[AttributeUsage(AttributeTargets.Class)]
public class DocIdAttribute : Attribute
{
    public string Id { get; }

    public DocIdAttribute(string id)
    {
        Id = id;
    }
}

public static class DocIdAtt
{
    
    public static string Get<T>()
    {
        var atts = (DocIdAttribute[])typeof(T).GetCustomAttributes(typeof(DocIdAttribute), true);
        if (atts.Length == 0)
            Guard.Against.AttributeNotDefined("DocId", atts, typeof(T).FullName);
        return atts[0].Id;
    }

    public static string Get(object obj)
    {
        var atts = (DocIdAttribute[])obj.GetType().GetCustomAttributes(typeof(DocIdAttribute), true);
        if (atts.Length == 0)
            Guard.Against.AttributeNotDefined("DocId", atts, obj.GetType().FullName);
        return atts[0].Id;
    }
    
}