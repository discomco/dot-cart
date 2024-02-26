using Ardalis.GuardClauses;

namespace DotCart.Abstractions.Behavior;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Field)]
public class CmdTopicAttribute : Attribute
{
    public CmdTopicAttribute(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}

public static class CmdTopicAtt
{
    public static string Get<T>()
    {
        var atts = (CmdTopicAttribute[])typeof(T).GetCustomAttributes(typeof(CmdTopicAttribute), true);
        Guard.Against.AttributeNotDefined("CmdTopic", atts, typeof(T).Name);
        return atts[0].Id;
    }

    public static string Get(object obj)
    {
        var atts = (CmdTopicAttribute[])obj.GetType().GetCustomAttributes(typeof(CmdTopicAttribute), true);
        Guard.Against.AttributeNotDefined("CmdTopic", atts, obj.GetType().Name);
        return atts[0].Id;
    }
}