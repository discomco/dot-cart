using Ardalis.GuardClauses;
using DotCart.Drivers.Ardalis;

namespace DotCart.Core;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class EndpointAttribute : Attribute
{
    public EndpointAttribute(string endpoint)
    {
        Endpoint = endpoint;
    }

    public string Endpoint { get; set; }
}

public static class EndpointAtt
{
    public static string Get<T>()
    {
        var atts = (EndpointAttribute[])typeof(T).GetCustomAttributes(typeof(EndpointAttribute),
            true);
        Guard.Against.AttributeNotDefined("Endpoint", atts, typeof(T).FullName);
        return atts[0].Endpoint;
    }

    public static string Get(object obj)
    {
        var atts = (EndpointAttribute[])obj.GetType().GetCustomAttributes(typeof(EndpointAttribute),
            true);
        Guard.Against.AttributeNotDefined("Endpoint", atts, obj.GetType().FullName);
        return atts[0].Endpoint;
    }
}