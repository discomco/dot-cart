using System.Runtime.Serialization;
using DotCart.Behavior;
using DotCart.Schema;
using DotCart.TestEnv.Engine.Schema;

namespace DotCart.TestEnv.Engine;

public static class ThrottleDown
{
    public const string CmdTopic = "engine:throttle_down:v1";
    public const string EvtTopic = "engine:throttled_down:v1";

    public record Payload(int Delta) : IPayload
    {
        public static Payload New(int delta)
        {
            return new Payload(delta);
        }
    }

    public record Evt(SimpleID AggregateId, Payload Payload) 
        : Evt<Payload>(EvtTopic, AggregateId, Payload)
    {
        public static Evt New(SimpleEngineID aggId, Payload payload)
        {
            return new Evt(aggId, payload);
        }
    }

    public record Cmd(IID AggregateID, Payload Payload) : Cmd<Payload>(CmdTopic, AggregateID, Payload)
    {
        public static Cmd New(IID aggID, Payload payload)
        {
            return new Cmd(aggID, payload);
        }
    }

    public class Exception : System.Exception
    {
        public Exception()
        {
        }

        protected Exception(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public Exception(string? message) : base(message)
        {
        }

        public Exception(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }

        public static Exception New(string msg)
        {
            return new Exception(msg);
        }
    }
}