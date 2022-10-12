using System.Runtime.Serialization;
using Ardalis.GuardClauses;
using DotCart.Contract;
using DotCart.Schema;
using DotCart.Schema.Tests;

namespace DotCart.Domain.Tests.Engine;

public partial class EngineAggregate
    : IExec<Schema.Tests.Engine, Initialize.Cmd>,
        IApply<Schema.Tests.Engine, Initialize.Evt>

{
    public IFeedback Verify(Schema.Tests.Engine state, Initialize.Cmd cmd)
    {
        var fbk = Feedback.New(cmd.AggregateID);
        try
        {
            Guard.Against.IsAlreadyInitialized(state);
        }
        catch (Exception e)
        {
            fbk.SetError(e.AsApiError());
            Console.WriteLine(e);
        }

        return fbk;
    }

    public IEnumerable<Domain.IEvt> Exec(Schema.Tests.Engine state, Initialize.Cmd cmd)
    {
        return new[]
        {
            new Initialize.Evt(cmd.AggregateID, Initialize.Payload.New(state))
        };
    }

    public Schema.Tests.Engine Apply(Schema.Tests.Engine state, Initialize.Evt evt)
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
    
        public static void IsAlreadyInitialized(this IGuardClause guard, Schema.Tests.Engine state)
        {
            if (((int)state.Status).HasFlag((int)EngineStatus.Initialized))
            {
                throw Excep.New($"Engine {state.Id} is already initialized.");
            }
        }

        public record Payload : IPld
        {
            public Schema.Tests.Engine Engine { get; }

            public static Payload New(Schema.Tests.Engine engine)
            {
                return new Payload(engine);
            }

            private Payload(Schema.Tests.Engine engine)
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