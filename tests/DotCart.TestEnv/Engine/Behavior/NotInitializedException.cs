using System.Runtime.Serialization;

namespace DotCart.TestEnv.Engine.Behavior;

public class NotInitializedException : Exception
{
    public NotInitializedException()
    {
    }

    protected NotInitializedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public NotInitializedException(string? message) : base(message)
    {
    }

    public NotInitializedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}