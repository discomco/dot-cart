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
using Serilog;

namespace Engine.Behavior;

public static class Stop
{

    public static readonly Evt2State<Engine, Evt> _evt2Doc = (state, evt) =>
    {
        state.Status = (Schema.EngineStatus)((int)state.Status).UnsetFlag((int)Schema.EngineStatus.Started);
        return state;
    };

    public static readonly Hope2Cmd<Cmd, Contract.Stop.Hope> _hope2Cmd = hope =>
        Cmd.New(hope.AggId.IDFromIdString(), hope.Payload);


    public static IServiceCollection AddStopBehavior(this IServiceCollection services)
    {
        return services
            .AddIDCtor()
            .AddBaseBehavior()
            .AddStopMappers()
            .AddTransient<ITry, TryCmd>()
            .AddTransient<IApply, ApplyEvt>();
    }

    public static IServiceCollection AddStopMappers(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2Doc)
            .AddTransient(_ => _hope2Cmd);
    }


    [Topic(Topics.Cmd_v1)]
    public record Cmd(IID AggregateID, Contract.Stop.Payload Payload)
        : CmdT<Contract.Stop.Payload>(AggregateID, Payload)
    {
        public static Cmd New(IID ID, Contract.Stop.Payload payload)
        {
            return new Cmd(ID, payload);
        }
    }

    public record EvtMeta(string AggregateType, string AggregateId)
        : EventMeta(AggregateType, AggregateId);

    [Topic(Topics.Evt_v1)]
    public record Evt(IID AggregateID,
            Contract.Stop.Payload Payload,
            EvtMeta Meta)
        : EvtT<Contract.Stop.Payload, EvtMeta>(
            AggregateID,
            TopicAtt.Get<Evt>(),
            Payload,
            Meta)
    {
        public static Evt New(IID AggregateID, Contract.Stop.Payload payload)
        {
            var meta = new EvtMeta(nameof(Aggregate), AggregateID.Id());
            return new Evt(AggregateID, payload, meta);
        }
    }

    public class TryCmd : TryCmdT<Cmd, Engine>
    {
        public override IFeedback Verify(Cmd cmd, Engine state)
        {
            var fbk = Feedback.New(cmd.AggregateID.Id());
            try
            {
                Guard.Against.EngineNotStarted(state);
            }
            catch (Exception e)
            {
                Log.Debug(e.InnerAndOuter());
                fbk.SetError(e.AsError());
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

    public class ApplyEvt : ApplyEvtT<Engine, Evt>
    {
        // public override Engine Apply(Engine state, Event evt)
        // {
        //     state.Status = (Schema.EngineStatus)((int)state.Status).UnsetFlag((int)Schema.EngineStatus.Started);
        //     state.Status = (Schema.EngineStatus)((int)state.Status).SetFlag((int)Schema.EngineStatus.Stopped);
        //     state.Power = 0;
        //     return state;
        // }
        public ApplyEvt(Evt2State<Engine, Evt> evt2State) : base(evt2State)
        {
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
    }

    public static class Topics
    {
        public const string Evt_v1 = "engine:stopped:v1";
        public const string Cmd_v1 = "engine:stop:v1";
    }
}