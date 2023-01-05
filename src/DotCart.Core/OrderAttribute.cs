using Ardalis.GuardClauses;
using DotCart.Drivers.Ardalis;

namespace DotCart.Core;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class OrderAttribute : Attribute
{
    public OrderAttribute(uint order)
    {
        Order = order;
    }

    public uint Order { get; }
}

public static class OrderAtt
{
    public static uint Get<T>()
    {
        var atts = (OrderAttribute[])typeof(T).GetCustomAttributes(typeof(OrderAttribute),
            true);
        Guard.Against.AttributeNotDefined("Order", atts, typeof(T).FullName);
        return atts[0].Order;
    }

    public static uint Get(object obj)
    {
        var atts = (OrderAttribute[])obj.GetType().GetCustomAttributes(typeof(OrderAttribute),
            true);
        Guard.Against.AttributeNotDefined("Order", atts, obj.GetType().FullName);
        return atts[0].Order;
    }
}