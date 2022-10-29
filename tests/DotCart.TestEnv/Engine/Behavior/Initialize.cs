using System.Runtime.Serialization;
using Ardalis.GuardClauses;
using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Schema;
using DotCart.TestEnv.Engine.Schema;

namespace DotCart.TestEnv.Engine.Behavior;

public partial class EngineAggregate
    : IExec<Initialize.Cmd>,
        IApply<Schema.Engine, Initialize.Evt>

{
    public IFeedback Verify(Initialize.Cmd cmd)
    {
        var fbk = Feedback.New(cmd.AggregateID);
        try
        {
            Guard.Against.IsAlreadyInitialized(_state);
        }
        catch (Exception e)
        {
            fbk.SetError(e.AsError());
            Console.WriteLine(e);
        }

        return fbk;
    }

    public IEnumerable<IEvt> Exec(Initialize.Cmd cmd)
    {
        return new[]
        {
            new Initialize.Evt(cmd.AggregateID, Initialize.Payload.New(_state))
        };
    }

    public IState Apply(Schema.Engine state, Initialize.Evt evt)
    {
        state.Id = evt.AggregateID.Value;
        state.Status = EngineStatus.Initialized;
        return state;
    }
}

public static class Initialize
{
    public const string CmdTopic = "test:engine:initialize:v1";
    public const string EvtTopic = "test:engine:initialized:v1";

    public static void IsAlreadyInitialized(this IGuardClause guard, Schema.Engine state)
    {
        if (((int)state.Status).HasFlag((int)EngineStatus.Initialized))
        {
            throw Excep.New($"Engine {state.Id} is already initialized.");
        }
    }

    public record Payload : IPayload
    {
        public Schema.Engine Engine { get; }

        public static Payload New(Schema.Engine engine)
        {
            return new Payload(engine);
        }

        private Payload(Schema.Engine engine)
        {
            Engine = engine;
        }
    }

    public class Excep : Exception
    {
        public Excep()
        {
        }

        protected Excep(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public Excep(string? message) : base(message)
        {
        }

        public Excep(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public static Excep New(string msg)
        {
            return new Excep(msg);
        }
    }

    public interface IEvt : IEvt<Payload>
    {
    }

    public record Evt
        (IID AggregateID, Payload Payload)
        : Evt<Payload>(EvtTopic, AggregateID, Payload), IEvt;

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