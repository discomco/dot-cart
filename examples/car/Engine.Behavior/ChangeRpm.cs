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
    private static readonly RaiseFuncT<Schema.Engine, Cmd>
        _raiseFunc =
            (cmd, state) =>
            {
                var res = new List<Event>();
                var rpmChanged = _newEvt(cmd.AggregateID, cmd.Payload, cmd.Meta);
                res.Add(rpmChanged);
                var newPower = state.Power + cmd.Payload.Delta;
                if (newPower > 0)
                    return res;
                var stopped = Stop._newEvt(
                    cmd.AggregateID,
                    Contract.Stop.Payload.New(),
                    EventMeta.New(
                        NameAtt.Get<IEngineAggregateInfo>(),
                        cmd.AggregateID.Id()
                    )
                );
                res.Add(stopped);
                return res;
            };

    private static readonly SpecFuncT<Schema.Engine, Cmd>
        _specFunc =
            (cmd, state) =>
            {
                var fbk = Feedback.New(cmd.AggregateID.Id());
                try
                {
                    Guard.Against.EngineNotStarted(state);
                }
                catch (Exception e)
                {
                    fbk.SetError(e.AsError());
                }

                return fbk;
            };

    private static readonly Evt2Doc<Schema.Engine, IEvt>
        _evt2State =
            (state, evt) =>
            {
                state.Power += evt.GetPayload<Contract.ChangeRpm.Payload>().Delta;
                return state;
            };

    private static readonly Evt2Fact<Contract.ChangeRpm.Fact, IEvt>
        _evt2Fact =
            evt => Contract.ChangeRpm.Fact.New(
                evt.AggregateId,
                evt.GetPayload<Contract.ChangeRpm.Payload>());

    private static readonly Hope2Cmd<Cmd, Contract.ChangeRpm.Hope>
        _hope2Cmd =
            hope => Cmd.New(
                hope.AggId.IDFromIdString(),
                hope.Payload,
                EventMeta.New(
                    NameAtt.Get<IEngineAggregateInfo>(),
                    hope.AggId)
            );

    public static EvtCtorT<IEvt, Contract.ChangeRpm.Payload, EventMeta>
        _newEvt =
            (id, payload, meta) => Event.New(
                id,
                TopicAtt.Get<IEvt>(),
                payload.ToBytes(),
                meta.ToBytes()
            );

    public static IServiceCollection AddChangeRpmMappers(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2State)
            .AddTransient(_ => _evt2Fact)
            .AddTransient(_ => _hope2Cmd);
    }

    public static IServiceCollection AddChangeRpmBehavior(this IServiceCollection services)
    {
        return services
            .AddRootDocCtors()
            .AddBaseBehavior<IEngineAggregateInfo, Schema.Engine, Cmd, IEvt>()
            .AddTransient(_ => _evt2State)
            .AddTransient(_ => _specFunc)
            .AddTransient(_ => _raiseFunc);
    }

    [Topic(Topics.Cmd_v1)]
    public record Cmd(IID AggregateID, Contract.ChangeRpm.Payload Payload, EventMeta Meta)
        : CmdT<Contract.ChangeRpm.Payload, EventMeta>(AggregateID, Payload, Meta)
    {
        public static Cmd New(IID aggregateID, Contract.ChangeRpm.Payload payload, EventMeta meta)
        {
            return new Cmd(aggregateID, payload, meta);
        }
    }

    [Topic(Topics.Evt_v1)]
    public interface IEvt : IEvtT<Contract.ChangeRpm.Payload>
    {
    }


// public record Evt(IID AggregateID, Contract.ChangeRpm.Payload Payload, EvtMeta Meta)
//     : EvtT<Contract.ChangeRpm.Payload, EvtMeta>(AggregateID, TopicAtt.Get<Evt>(), Payload, Meta)
// {
//     public static Evt New(IID engineID, Contract.ChangeRpm.Payload payload)
//     {
//         var meta = new EvtMeta(NameAtt.Get<IEngineAggregateInfo>(), engineID.Id());
//         return new Evt(engineID, payload, meta);
//     }
// }

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

    public static class Topics
    {
        public const string Cmd_v1 = "engine:change_rpm:v1";
        public const string Evt_v1 = "engine:rpm_changed:v1";
    }
}