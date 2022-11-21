using System.Runtime.Serialization;

namespace DotCart.Drivers.EventStoreDB;

public class ESDBException : Exception
{
    public ESDBException()
    {
    }

    protected ESDBException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ESDBException(string? message) : base(message)
    {
    }

    public ESDBException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}