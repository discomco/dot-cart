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
        if (atts.Length == 0) throw new Exception($"Attribute 'Tag' is not defined on {typeof(T)}!");
        return atts[0].Id;
    }

    public static string Get(object obj)
    {
        var atts = (TagAttribute[])obj.GetType().GetCustomAttributes(typeof(TagAttribute), true);
        if (atts.Length == 0) throw new Exception($"Attribute 'Tag' is not defined on {obj.GetType()}!");
        return atts[0].Id;
    }

    public static string GetFromField<T>(string fieldName)
    {
        return AttributeHelper.GetConstFieldAttributeValue<T, string, TagAttribute>(
            fieldName, y => y.Id);
    }
    
    
    
}