using System.Runtime.Serialization;

namespace DotCart.Abstractions.Schema;

public class IDPrefixNotSetException : Exception
{
    public IDPrefixNotSetException()
    {
    }

    protected IDPrefixNotSetException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public IDPrefixNotSetException(string? message) : base(message)
    {
    }

    public IDPrefixNotSetException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public static IDPrefixNotSetException New => new("[IDPrefix] attribute not set on the ID");
}