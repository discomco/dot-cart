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

public static class Start
{
    public const string CmdTopic_v1 = "engine:start:v1";
    public const string EvtTopic_v1 = "engine:started:v1";

    private static readonly Evt2Fact<Contract.Start.Fact, IEvt> _evt2Fact =
        evt => Contract.Start.Fact.New(evt.AggregateID.Id(), evt.GetPayload<Contract.Start.Payload>());

    private static readonly Hope2Cmd<Cmd, Contract.Start.Hope> _hope2Cmd =
        hope =>
            Cmd.New(hope.AggId.IDFromIdString(), hope.Payload);

    private static readonly Evt2State<Engine, IEvt> _evt2Doc =
        (state, _) =>
        {
            state.Status = state.Status.SetFlag(Schema.EngineStatus.Started);
            return state;
        };

    private static readonly Evt2Cmd<Cmd, Initialize.IEvt> _evt2Cmd =
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

    public class ApplyEvt : ApplyEvtT<Engine, IEvt>
    {
        public override Engine Apply(Engine state, Event evt)
        {
            state.Status = (Schema.EngineStatus)((int)state.Status).SetFlag((int)Schema.EngineStatus.Started);
            return state;
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
                Event.New(
                    cmd.AggregateID,
                    TopicAtt.Get<IEvt>(),
                    cmd.Payload.ToBytes(),
                    Aggregate.GetMeta().ToBytes(),
                    Aggregate.Version,
                    DateTime.UtcNow)
            };
        }
    }

    public class StartOnInitializedPolicy : AggregatePolicy<Initialize.IEvt, Cmd>
    {
        public StartOnInitializedPolicy(
            IExchange exchange,
            Evt2Cmd<Cmd, Initialize.IEvt> evt2Cmd)
            : base(exchange, evt2Cmd)
        {
        }
    }

    [Topic(CmdTopic_v1)]
    public record Cmd(IID AggregateID, Contract.Start.Payload Payload)
        : CmdT<Contract.Start.Payload>(AggregateID, Payload)
    {
        public static Cmd New(IID aggregateID, Contract.Start.Payload payload)
        {
            return new Cmd(aggregateID, payload);
        }
    }

    [Topic(EvtTopic_v1)]
    public interface IEvt : IEvtT<Contract.Start.Payload>
    {
    }
}