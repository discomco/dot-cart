using System.Runtime.Serialization;
using Ardalis.GuardClauses;
using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Schema;
using DotCart.TestEnv.Engine.Behavior;

namespace DotCart.TestEnv.Engine;

public partial class Aggregate :
    ITry<ThrottleUp.Cmd>,
    IApply<Schema.Engine, ThrottleUp.Evt>
{
    public IState Apply(Schema.Engine state, ThrottleUp.Evt evt)
    {
        state.Power += evt.Payload.Delta;
        return state;
    }

    public IFeedback Verify(ThrottleUp.Cmd cmd)
    {
        var fbk = Feedback.New(cmd.AggregateID);
        try
        {
            Guard.Against.EngineNotStarted(_state);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return fbk;
    }

    public IEnumerable<IEvt> Raise(ThrottleUp.Cmd cmd)
    {
        return new[]
        {
            ThrottleUp.Evt.New(cmd.AggregateID, cmd.Payload)
        };
    }
}

public static class ThrottleUp
{
    public const string CmdTopic = "engine:trottle_up:v1";
    public const string EvtTopic = "engine:throttled_Up:v1";

    public record Payload(int Delta) : IPayload
    {
        public static Payload New(int delta)
        {
            return new Payload(delta);
        }
    }

    public record Cmd(IID AggregateID, Payload Payload)
        : Cmd<Payload>(CmdTopic, AggregateID, Payload)
    {
        public static Cmd New(IID aggID, Payload payload)
        {
            return new Cmd(aggID, payload);
        }
    }

    public record Evt(IID AggregateID, Payload Payload)
        : Evt<Payload>(EvtTopic, AggregateID, Payload)
    {
        public static IEvt New(IID aggID, Payload payload)
        {
            return new Evt(aggID, payload);
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