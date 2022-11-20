using Ardalis.GuardClauses;
using DotCart.Drivers.Ardalis;

namespace DotCart.Abstractions.Drivers;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class StreamNameAttribute : Attribute
{
    public StreamNameAttribute(string streamName)
    {
        StreamName = streamName;
    }

    public string StreamName { get; }
}

public static class StreamName
{
    public static string Get<TDriver>() where TDriver : IProjectorDriver
    {
        var atts = (StreamNameAttribute[])typeof(TDriver).GetCustomAttributes(typeof(StreamNameAttribute),
            true);
        Guard.Against.AttributeNotDefined("StreamName", atts, typeof(TDriver).Name);
        return atts[0].StreamName;
    }
}