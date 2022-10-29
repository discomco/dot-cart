namespace DotCart.Drivers.EventStoreDB;

[AttributeUsage(AttributeTargets.Class)]
public class StreamNameAttribute : Attribute
{
    public StreamNameAttribute(string streamName)
    {
        StreamName = streamName;
    }

    public string StreamName { get; }
}