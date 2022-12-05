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
            .AddStateCtor()
            .AddBaseBehavior<IEngineAggregateInfo, Engine, Cmd, Evt>()
            .AddTransient(_ => _evt2State)
            .AddTransient(_ => _specFunc)
            .AddTransient(_ => _raiseFunc);
    }

    private static readonly RaiseFuncT<Engine, Cmd>
        _raiseFunc =
            (cmd, state) =>
            {
                var res = new List<Event>();
                var rpmChanged = Evt.New(cmd.AggregateID, cmd.Payload);
                res.Add(rpmChanged);
                var newPower = state.Power + cmd.Payload.Delta;
                if (newPower > 0)
                    return res;
                var stopped = Stop.Evt.New(cmd.AggregateID, Contract.Stop.Payload.New());
                res.Add(stopped);
                return res;
            };

    private static readonly SpecFuncT<Engine, Cmd>
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

    private static readonly Evt2State<Engine, Evt>
        _evt2State =
            (state, evt) =>
            {
                state.Power += evt.GetPayload<Contract.ChangeRpm.Payload>().Delta;
                return state;
            };

    private static readonly Evt2Fact<Contract.ChangeRpm.Fact, Evt>
        _evt2Fact =
            evt => Contract.ChangeRpm.Fact.New(
                evt.AggregateID.Id(),
                evt.GetPayload<Contract.ChangeRpm.Payload>());

    private static readonly Hope2Cmd<Cmd, Contract.ChangeRpm.Hope>
        _hope2Cmd =
            hope => Cmd.New(hope.AggId.IDFromIdString(), hope.Payload);

    public record EvtMeta(string AggregateType, string AggregateId)
        : EventMeta(AggregateType, AggregateId);
    [Topic(Topics.Cmd_v1)]
    public record Cmd(IID AggregateID, Contract.ChangeRpm.Payload Payload)
        : CmdT<Contract.ChangeRpm.Payload>(AggregateID, Payload)
    {
        public static Cmd New(IID aggregateID, Contract.ChangeRpm.Payload payload)
        {
            return new Cmd(aggregateID, payload);
        }
    }
    [Topic(Topics.Evt_v1)]
    public record Evt(IID AggregateID, Contract.ChangeRpm.Payload Payload, EvtMeta Meta)
        : EvtT<Contract.ChangeRpm.Payload, EvtMeta>(AggregateID, TopicAtt.Get<Evt>(), Payload, Meta)
    {
        public static Evt New(IID engineID, Contract.ChangeRpm.Payload payload)
        {
            var meta = new EvtMeta(NameAtt.Get<IEngineAggregateInfo>(), engineID.Id());
            return new Evt(engineID, payload, meta);
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
    public static class Topics
    {
        public const string Cmd_v1 = "engine:change_rpm:v1";
        public const string Evt_v1 = "engine:changed_rpm:v1";
    }
}