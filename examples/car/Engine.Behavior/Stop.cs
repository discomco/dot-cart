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
    public const string EvtTopic = "engine:stopped:v1";
    public const string CmdTopic = "engine:stop:v1";

    public static readonly Evt2State<Engine, IEvt> _evt2Doc = (state, evt) =>
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


    [Topic(CmdTopic)]
    public record Cmd(ID AggregateID, Contract.Stop.Payload Payload)
        : CmdT<Contract.Stop.Payload>(AggregateID, Payload)
    {
        public static Cmd New(ID ID, Contract.Stop.Payload payload)
        {
            return new Cmd(ID, payload);
        }
    }

    [Topic(EvtTopic)]
    public interface IEvt : IEvtT<Contract.Stop.Payload>
    {
    }

    [Topic(EvtTopic)]
    public record Evt(ID AggregateID,
            long Version,
            Contract.ChangeRpm.Payload Payload,
            byte[] MetaData,
            DateTime TimeStamp)
        : Event(AggregateID, EvtTopic, Version, Payload.ToBytes(), MetaData, TimeStamp)
    {
        public static Evt New(ID AggregateID,
            long Version,
            Contract.ChangeRpm.Payload payload,
            byte[] MetaData,
            DateTime TimeStamp)
        {
            return new Evt(AggregateID, Version, payload, MetaData, TimeStamp);
        }
    }

    public class TryCmd : TryCmdT<Cmd>
    {
        public override IFeedback Verify(Cmd cmd)
        {
            var fbk = Feedback.New(cmd.AggregateID.Id());
            try
            {
                Guard.Against.EngineNotStarted((Engine)Aggregate.GetState());
            }
            catch (Exception e)
            {
                Log.Debug(e.InnerAndOuter());
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
                    cmd.Payload,
                    Aggregate.GetMeta(),
                    Aggregate.Version
                )
            };
        }
    }

    public class ApplyEvt : ApplyEvtT<Engine, IEvt>
    {
        public override Engine Apply(Engine state, Event evt)
        {
            state.Status = (Schema.EngineStatus)((int)state.Status).UnsetFlag((int)Schema.EngineStatus.Started);
            state.Status = (Schema.EngineStatus)((int)state.Status).SetFlag((int)Schema.EngineStatus.Stopped);
            state.Power = 0;
            return state;
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
}