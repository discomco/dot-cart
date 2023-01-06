using Ardalis.GuardClauses;
using DotCart.Drivers.Ardalis;

namespace DotCart.Core;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class NameAttribute : Attribute
{
    public NameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}

public static class NameAtt
{
    public static string Get<T>()
    {
        var atts = (NameAttribute[])typeof(T).GetCustomAttributes(typeof(NameAttribute),
            true);
        Guard.Against.AttributeNotDefined("Name", atts, typeof(T).FullName);
        return atts[0].Name;
    }

    public static string Get(object obj)
    {
        var atts = (NameAttribute[])obj.GetType().GetCustomAttributes(typeof(NameAttribute),
            true);
        Guard.Against.AttributeNotDefined("Name", atts, obj.GetType().FullName);
        return atts[0].Name;
    }

    public static string ChoreographyName<TEvtPayload, TCmdPayload>()
    {
        return $"should ({CmdTopicAtt.Get<TCmdPayload>()}) on ({EvtTopicAtt.Get<TEvtPayload>()})";
    }

    public static string StepName(Importance level, string stepName, string payloadTopic)
    {
        return $"[{level}] step {stepName}({payloadTopic})";
    }
}