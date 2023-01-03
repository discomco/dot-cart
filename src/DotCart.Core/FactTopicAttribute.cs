using Ardalis.GuardClauses;
using DotCart.Drivers.Ardalis;

namespace DotCart.Core;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Field)]
public class FactTopicAttribute : Attribute
{
    public FactTopicAttribute(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}

public static class FactTopicAtt
{
    public static string Get<T>()
    {
        var atts = (FactTopicAttribute[])typeof(T).GetCustomAttributes(typeof(FactTopicAttribute), true);
        Guard.Against.AttributeNotDefined("FactTopic", atts, typeof(T).Name);
        return atts[0].Id;
    }

    public static string Get(object obj)
    {
        var atts = (FactTopicAttribute[])obj.GetType().GetCustomAttributes(typeof(FactTopicAttribute), true);
        Guard.Against.AttributeNotDefined("FactTopic", atts, obj.GetType().Name);
        return atts[0].Id;
    }
}