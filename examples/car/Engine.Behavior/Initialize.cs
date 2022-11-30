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
    public const string CmdTopic = "engine:initialize:v1";
    public const string EvtTopic = "engine:initialized:v1";

    private static readonly Evt2Fact<Contract.Initialize.Fact, IEvt> _evt2Fact =
        evt => Contract.Initialize.Fact.New(evt.AggregateID.Id(), evt.GetPayload<Contract.Initialize.Payload>());

    private static readonly Hope2Cmd<Cmd, Contract.Initialize.Hope> _hope2Cmd =
        hope => Cmd.New(hope.Payload);


    private static readonly Evt2State<Engine, IEvt> _evt2Doc = (state, evt) =>
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

    public class ApplyEvt : ApplyEvtT<Engine, IEvt>
    {
        public override Engine Apply(Engine state, Event evt)
        {
            state.Id = evt.AggregateID.Id();
            state.Status = Schema.EngineStatus.Initialized;
            return state;
        }
    }

    public class TryCmd : TryCmdT<Cmd>
    {
        public override IFeedback Verify(Cmd cmd)
        {
            var fbk = Feedback.New(cmd.AggregateID.Id());
            try
            {
                Guard.Against.EngineInitialized((Engine)Aggregate.GetState());
            }
            catch (Exception e)
            {
                fbk.SetError(e.AsError());
                Console.WriteLine(e);
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
                // Evt(, Payload.New(cmd.Payload.Engine))
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

    [Topic(EvtTopic)]
    public interface IEvt : IEvtT<Contract.Initialize.Payload>
    {
    }

    
    [Topic(CmdTopic)]
    public record Cmd(Contract.Initialize.Payload Payload)
        : CmdT<Contract.Initialize.Payload>(Schema.EngineID.New(), Payload)
    {
        public static Cmd New(Contract.Initialize.Payload payload)
        {
            return new Cmd(payload);
        }
    }
}