using System.Reflection;

namespace DotCart.Core;

public static class AttributeHelper
{
    public static TOut? GetConstFieldAttributeValue<T, TOut, TAttribute>(
        string fieldName,
        Func<TAttribute, TOut> valueSelector)
        where TAttribute : Attribute
    {
        var fieldInfo = typeof(T).GetField(fieldName, BindingFlags.Public | BindingFlags.Static);
        if (fieldInfo == null)
        {
            return default;
        }
        return fieldInfo.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute att 
            ? valueSelector(att) 
            : default;
    }
}