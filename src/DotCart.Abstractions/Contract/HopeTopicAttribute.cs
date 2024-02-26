using Ardalis.GuardClauses;

namespace DotCart.Abstractions.Contract;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Field)]
public class HopeTopicAttribute : Attribute
{
    public HopeTopicAttribute(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}

public static class HopeTopicAtt
{
    public static string Get<T>()
    {
        var atts = (HopeTopicAttribute[])typeof(T).GetCustomAttributes(typeof(HopeTopicAttribute), true);
        Guard.Against.AttributeNotDefined("HopeTopic", atts, typeof(T).Name);
        return atts[0].Id;
    }

    public static string Get(object obj)
    {
        var atts = (HopeTopicAttribute[])obj.GetType().GetCustomAttributes(typeof(HopeTopicAttribute), true);
        Guard.Against.AttributeNotDefined("HopeTopic", atts, obj.GetType().Name);
        return atts[0].Id;
    }
}