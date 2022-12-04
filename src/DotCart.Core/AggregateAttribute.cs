namespace DotCart.Core;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class AggregateAttribute : Attribute
{
    public string Name { get; set; }

    public AggregateAttribute(string name)
    {
        Name = name;
    }
    
}


public static class AggregateAtt
{
    public static string Get<T>()
    {
        var atts = (AggregateAttribute[])typeof(T).GetCustomAttributes(typeof(AggregateAttribute), true);
        if (atts.Length == 0) throw new Exception($"Attribute 'Aggregate' is not defined on {typeof(T)}!");
        return atts[0].Name;
    }

    public static string Get(object obj)
    {
        var atts = (AggregateAttribute[])obj.GetType().GetCustomAttributes(typeof(AggregateAttribute), true);
        if (atts.Length == 0) throw new Exception($"Attribute 'Aggregate' is not defined on {obj.GetType()}!");
        return atts[0].Name;
    }
}