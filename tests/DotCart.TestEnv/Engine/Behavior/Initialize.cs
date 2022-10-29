using System.Runtime.Serialization;
using Ardalis.GuardClauses;
using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Schema;
using DotCart.TestEnv.Engine.Schema;

namespace DotCart.TestEnv.Engine.Behavior;

public partial class EngineAggregate
    : ITry<Initialize.Cmd>,
        IApply<Schema.Engine, Initialize.Evt>

{
    public IState Apply(Schema.Engine state, Initialize.Evt evt)
    {
        state.Id = evt.AggregateID.Value;
        state.Status = EngineStatus.Initialized;
        return state;
    }

    public IFeedback Verify(Initialize.Cmd cmd)
    {
        var fbk = Feedback.New(cmd.AggregateID);
        try
        {
            Guard.Against.EngineInitialized(_state);
        }
        catch (Exception e)
        {
            fbk.SetError(e.AsError());
            Console.WriteLine(e);
        }

        return fbk;
    }

    public IEnumerable<IEvt> Raise(Initialize.Cmd cmd)
    {
        return new[]
        {
            new Initialize.Evt(cmd.AggregateID, Initialize.Payload.New(cmd.Payload.Engine))
        };
    }
}

public static class Initialize
{
    public const string CmdTopic = "test:engine:initialize:v1";
    public const string EvtTopic = "test:engine:initialized:v1";


    public record Payload : IPayload
    {
        private Payload(Schema.Engine engine)
        {
            Engine = engine;
        }

        public Schema.Engine Engine { get; }

        public static Payload New(Schema.Engine engine)
        {
            return new Payload(engine);
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

    public interface IEvt : IEvt<Payload>
    {
    }

    public record Evt(IID AggregateID, Payload Payload)
        : Evt<Payload>(EvtTopic, AggregateID, Payload), IEvt
    {
        public static Evt New(EngineID engineId, Payload initPayload)
        {
            return new Evt(engineId, initPayload);
        }
    }

    public interface ICmd : ICmd<Payload>
    {
    }

    public record Cmd(IID AggregateID, Payload Payload)
        : Cmd<Payload>(CmdTopic, AggregateID, Payload), ICmd
    {
        public static ICmd New(EngineID engineId, Payload payload)
        {
            return new Cmd(engineId, payload);
        }
    }
}