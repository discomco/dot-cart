using System.Runtime.Serialization;

namespace DotCart.Drivers.EventStoreDB;

public class ESDBEventStoreDriverException : Exception
{
    public ESDBEventStoreDriverException()
    {
    }

    protected ESDBEventStoreDriverException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ESDBEventStoreDriverException(string? message) : base(message)
    {
    }

    public ESDBEventStoreDriverException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}