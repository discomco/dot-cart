namespace DotCart.Core;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Field)]
public class TopicAttribute : Attribute
{
    public TopicAttribute(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}

public static class TopicAtt
{
    public static string Get<T>()
    {
        var atts = (TopicAttribute[])typeof(T).GetCustomAttributes(typeof(TopicAttribute), true);
        if (atts.Length == 0)
            throw new Exception($"Attribute 'Topic' is not defined on {typeof(T)}!");
        return atts[0].Id;
    }

    public static string Get(object obj)
    {
        var atts = (TopicAttribute[])obj.GetType().GetCustomAttributes(typeof(TopicAttribute), true);
        if (atts.Length == 0)
            throw new Exception($"Attribute 'Topic' is not defined on {obj.GetType()}!");
        return atts[0].Id;
    }
}