using Ardalis.GuardClauses;

namespace DotCart.Abstractions.Behavior;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Field)]
public class EvtTopicAttribute : Attribute
{
    public EvtTopicAttribute(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}

public static class EvtTopicAtt
{
    public static string Get<T>()
    {
        var atts = (EvtTopicAttribute[])typeof(T).GetCustomAttributes(typeof(EvtTopicAttribute), true);
        Guard.Against.AttributeNotDefined("EvtTopic", atts, typeof(T).Name);
        return atts[0].Id;
    }

    public static string Get(object obj)
    {
        var atts = (EvtTopicAttribute[])obj.GetType().GetCustomAttributes(typeof(EvtTopicAttribute), true);
        Guard.Against.AttributeNotDefined("EvtTopic", atts, obj.GetType().Name);
        return atts[0].Id;
    }
}