using Ardalis.GuardClauses;

namespace DotCart.Core;

[AttributeUsage(AttributeTargets.All)]
public class TagAttribute : Attribute
{
    public TagAttribute(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}

public static class TagAtt
{
    public static string Get<T>()
    {
        var atts = (TagAttribute[])typeof(T).GetCustomAttributes(typeof(TagAttribute), true);
        Guard.Against.AttributeNotDefined("Tag", atts, typeof(T).Name);
        return atts[0].Id;
    }

    public static string Get(object obj)
    {
        var atts = (TagAttribute[])obj.GetType().GetCustomAttributes(typeof(TagAttribute), true);
        Guard.Against.AttributeNotDefined("Tag", atts, obj.GetType().Name);
        return atts[0].Id;
    }

    public static string GetFromField<T>(string fieldName)
    {
        return AttributeHelper.GetConstFieldAttributeValue<T, string, TagAttribute>(
            fieldName, y => y.Id);
    }
}