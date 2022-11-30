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

public static class ChangeRpm
{
    public const string CmdTopic = "engine:change_rpm:v1";
    public const string EvtTopic = "engine:changed_rpm:v1";

    private static readonly Evt2State<Engine, IEvt> _evt2Doc = (state, evt) =>
    {
        state.Power += evt.GetPayload<Contract.ChangeRpm.Payload>().Delta;
        return state;
    };

    private static Evt2Fact<Contract.ChangeRpm.Fact, IEvt> _evt2Fact =>
        evt => Contract.ChangeRpm.Fact.New(
            evt.AggregateID.Id(),
            evt.GetPayload<Contract.ChangeRpm.Payload>());

    private static Hope2Cmd<Cmd, Contract.ChangeRpm.Hope> _hope2Cmd =>
        hope => Cmd.New(hope.AggId.IDFromIdString(), hope.Payload);

    private static IServiceCollection AddChangeRpmMappers(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2Fact)
            .AddTransient(_ => _evt2Doc)
            .AddTransient(_ => _hope2Cmd);
    }


    public static IServiceCollection AddChangeRpmBehavior(this IServiceCollection services)
    {
        return services
            .AddIDCtor()
            .AddBaseBehavior()
            .AddChangeRpmMappers()
            .AddTransient<ITry, TryCmd>()
            .AddTransient<IApply, ApplyEvt>();
    }


    [Topic(CmdTopic)]
    public record Cmd(IID AggregateID, Contract.ChangeRpm.Payload Payload)
        : CmdT<Contract.ChangeRpm.Payload>(AggregateID, Payload), ICmd
    {
        public static Cmd New(IID aggregateID, Contract.ChangeRpm.Payload payload)
        {
            return new Cmd(aggregateID, payload);
        }
    }

    [Topic(EvtTopic)]
    public interface IEvt : IEvtT<Contract.ChangeRpm.Payload>
    {
    }

    public class TryCmd : TryCmdT<Cmd>
    {
        public override IEnumerable<Event> Raise(Cmd cmd)
        {
            var res = new List<Event>();
            var rpmChanged = Event.New(cmd.AggregateID,
                TopicAtt.Get<IEvt>(),
                cmd.Payload.ToBytes(),
                Aggregate.GetMeta().ToBytes(),
                Aggregate.Version,
                DateTime.UtcNow);
            res.Add(rpmChanged);
            var newPower = ((Engine)Aggregate.GetState()).Power + cmd.Payload.Delta;
            if (newPower > 0)
                return res;
            var stopped = Event.New(
                cmd.AggregateID,
                TopicAtt.Get<Behavior.Stop.IEvt>(),
                Contract.Stop.Payload.New().ToBytes(),
                Aggregate.GetMeta().ToBytes(),
                Aggregate.Version,
                DateTime.UtcNow);
            res.Add(stopped);
            return res;
        }

        public override IFeedback Verify(Cmd cmd)
        {
            var fbk = Feedback.New(cmd.AggregateID.Id());
            try
            {
                Guard.Against.EngineNotStarted((Engine)Aggregate.GetState());
            }
            catch (Exception e)
            {
                fbk.SetError(e.AsError());
            }

            return fbk;
        }
    }

    public class ApplyEvt : ApplyEvtT<Engine, IEvt>
    {
        public override Engine Apply(Engine state, Event evt)
        {
            state.Power += evt.GetPayload<Contract.ChangeRpm.Payload>().Delta;
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

        public static Exception New(string msg)
        {
            return new Exception(msg);
        }
    }
}