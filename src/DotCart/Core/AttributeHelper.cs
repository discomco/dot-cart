using System.Reflection;
using DotCart.Abstractions;
using DotCart.Abstractions.Behavior;

namespace DotCart.Core;

public static class AttributeHelper
{
    public static TOut? GetConstFieldAttributeValue<T, TOut, TAttribute>(
        string fieldName,
        Func<TAttribute, TOut> valueSelector)
        where TAttribute : Attribute
    {
        var fieldInfo = typeof(T).GetField(fieldName, BindingFlags.Public | BindingFlags.Static);
        if (fieldInfo == null) return default;
        return fieldInfo.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute att
            ? valueSelector(att)
            : default;
    }
}

public static class NameAtt2
{
    public static string ChoreographyName<TEvtPayload, TCmdPayload>()
    {
        return $"should ({CmdTopicAtt.Get<TCmdPayload>()}) on ({EvtTopicAtt.Get<TEvtPayload>()})";
    }

    public static string StepName(
        Importance level,
        string stepName,
        string payloadTopic
    )
    {
        return $"[{level}] step {stepName}({payloadTopic})";
    }
}