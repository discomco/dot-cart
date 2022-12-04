using System.Runtime.Serialization;
using Ardalis.GuardClauses;
using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behaviors;
using DotCart.Core;
using Elasticsearch.Net;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Behavior;

public static class Start
{
    public static class Topics
    {
        public const string Cmd_v1 = "engine:start:v1";
        public const string Evt_v1 = "engine:started:v1";
    }


    private static readonly Evt2Fact<Contract.Start.Fact, Evt> _evt2Fact =
        evt => Contract.Start.Fact.New(evt.AggregateID.Id(), evt.GetPayload<Contract.Start.Payload>());

    private static readonly Hope2Cmd<Cmd, Contract.Start.Hope> _hope2Cmd =
        hope =>
            Cmd.New(hope.AggId.IDFromIdString(), hope.Payload);

    private static readonly Evt2State<Engine, Evt> _evt2Doc =
        (state, _) =>
        {
            state.Status = state.Status.SetFlag(Schema.EngineStatus.Started);
            return state;
        };

    private static readonly Evt2Cmd<Cmd, Initialize.Evt> _evt2Cmd =
        evt =>
            Cmd.New(evt.AggregateID, Contract.Start.Payload.New);

    private static IServiceCollection AddStartMappers(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2Cmd)
            .AddTransient(_ => _evt2Fact)
            .AddTransient(_ => _evt2Doc)
            .AddTransient(_ => _hope2Cmd);
    }

    public static IServiceCollection AddStartBehavior(this IServiceCollection services)
    {
        return services
            .AddIDCtor()
            .AddBaseBehavior()
            .AddStartMappers()
            .AddTransient<IAggregatePolicy, StartOnInitializedPolicy>()
            .AddTransient<ITry, TryCmd>()
            .AddTransient<IApply, ApplyEvt>();
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

        public static Exception New(string message)
        {
            return new Exception(message);
        }
    }

    public class ApplyEvt : ApplyEvtT<Engine, Evt>
    {
        // public override Engine Apply(Engine state, Event evt)
        // {
        //     state.Status = (Schema.EngineStatus)((int)state.Status).SetFlag((int)Schema.EngineStatus.Started);
        //     return state;
        // }
        public ApplyEvt(Evt2State<Engine, Evt> evt2State) : base(evt2State)
        {
        }
    }

    public class TryCmd : TryCmdT<Cmd>
    {
        public override IFeedback Verify(Cmd cmd)
        {
            var fbk = Feedback.Empty;
            try
            {
                Guard.Against.StateIsNotInitialized((Engine)Aggregate.GetState());
            }
            catch (Exception e)
            {
                fbk.SetError(e.AsError());
            }

            return fbk;
        }

        public override IEnumerable<Event> Raise(Cmd cmd)
        {
            return new[]
            {
                Evt.New(cmd.AggregateID, cmd.Payload)
            };
        }
    }

    public class StartOnInitializedPolicy : AggregatePolicy<Initialize.Evt, Cmd>
    {
        public StartOnInitializedPolicy(
            IExchange exchange,
            Evt2Cmd<Cmd, Initialize.Evt> evt2Cmd)
            : base(exchange, evt2Cmd)
        {
        }
    }

    [Topic(Topics.Cmd_v1)]
    public record Cmd(IID AggregateID, Contract.Start.Payload Payload)
        : CmdT<Contract.Start.Payload>(AggregateID, Payload)
    {
        public static Cmd New(IID aggregateID, Contract.Start.Payload payload)
        {
            return new Cmd(aggregateID, payload);
        }
    }

    public record EvtMeta(string AggregateType, string AggregateId)
        : EventMeta(AggregateType, AggregateId);

    [Topic(Topics.Evt_v1)]
    public record Evt(IID AggregateID,
        Contract.Start.Payload Payload,
        EvtMeta Meta) : EvtT<Contract.Start.Payload, EvtMeta>(AggregateID,
        TopicAtt.Get<Evt>(),
        Payload,
        Meta)
    {
        public static Evt New(IID aggregateID, Contract.Start.Payload payload)
        {
            var meta = new EvtMeta(nameof(Aggregate), aggregateID.Id());
            return new Evt(aggregateID, payload, meta);
        }
    }
}

