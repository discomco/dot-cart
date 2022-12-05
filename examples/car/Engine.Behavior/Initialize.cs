using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;
using Ardalis.GuardClauses;
using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behaviors;
using DotCart.Core;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Behavior;

public static class Initialize
{

    private static readonly Evt2Fact<Contract.Initialize.Fact, Evt> _evt2Fact =
        evt => Contract.Initialize.Fact.New(evt.AggregateID.Id(), evt.GetPayload<Contract.Initialize.Payload>());

    private static readonly Hope2Cmd<Cmd, Contract.Initialize.Hope> _hope2Cmd =
        hope => Cmd.New(hope.Payload);


    private static readonly Evt2State<Engine, Behavior.Initialize.Evt> _evt2Doc = (state, evt) =>
    {
        if (evt == null) return state;
        if (evt.GetPayload<Engine>() == null) return state;
        state = evt.GetPayload<Engine>();
        state.Id = evt.AggregateID.Id();
        state.Status = Schema.EngineStatus.Initialized;
        return state;
    };

    private static IServiceCollection AddInitializeMappers(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2Fact)
            .AddTransient(_ => _evt2Doc)
            .AddTransient(_ => _hope2Cmd);
    }


    public static IServiceCollection AddInitializeBehavior(this IServiceCollection services)
    {
        return services
            .AddIDCtor()
            .AddBaseBehavior()
            .AddInitializeMappers()
            .AddTransient<ITry, TryCmd>()
            .AddTransient<IApply, ApplyEvt>();
    }

    public class ApplyEvt : ApplyEvtT<Engine, Evt>
    {
        // public override Engine Apply(Engine state, Event evt)
        // {
        //     state.Id = evt.AggregateID.Id();
        //     state.Status = Schema.EngineStatus.Initialized;
        //     return state;
        // }
        public ApplyEvt(Evt2State<Engine, Evt> evt2State) : base(evt2State)
        {
        }
    }

    public class TryCmd : TryCmdT<Cmd, Engine>
    {
        public override IFeedback Verify(Cmd cmd, Engine state)
        {
            var fbk = Feedback.New(cmd.AggregateID.Id());
            try
            {
                Guard.Against.EngineInitialized(state);
            }
            catch (Exception e)
            {
                fbk.SetError(e.AsError());
                Console.WriteLine(e);
            }

            return fbk;
        }

        public override IEnumerable<Event> Raise(Cmd cmd, Engine state)
        {
            return new[]
            {
                Evt.New(cmd.AggregateID, cmd.Payload)
            };
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


    public record EvtMeta(string AggregateType, string AggregateId)
        : EventMeta(AggregateType, AggregateId);

    [Topic(Topics.Evt_v1)]
    public record Evt(IID AggregateID,
        Contract.Initialize.Payload Payload,
        EvtMeta Meta) : EvtT<
        Contract.Initialize.Payload,
        EvtMeta>(AggregateID,
        TopicAtt.Get<Evt>(),
        Payload,
        Meta)
    {
        public static Evt New(IID aggregateID, Contract.Initialize.Payload payload)
        {
            var meta = new EvtMeta(nameof(Aggregate), aggregateID.Id());
            return new Evt(aggregateID, payload, meta);
        }
    }


    [Topic(Topics.Cmd_v1)]
    public record Cmd(Contract.Initialize.Payload Payload)
        : CmdT<Contract.Initialize.Payload>(Schema.EngineID.New(), Payload)
    {
        public static Cmd New(Contract.Initialize.Payload payload)
        {
            return new Cmd(payload);
        }
    }

    public static class Topics
    {
        public const string Cmd_v1 = "engine:initialize:v1";
        public const string Evt_v1 = "engine:initialized:v1";
    }
}