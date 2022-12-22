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
    public const string OnZeroPower_v1 = "engine:on_zero_power:stop:v1";


    private static readonly Evt2Fact<Contract.Stop.Fact, IEvt>
        _evt2Fact =
            evt => Contract.Stop.Fact.New(
                evt.AggregateId,
                evt.GetPayload<Contract.Stop.Payload>()
            );

    private static readonly Evt2State<Engine, IEvt>
        _evt2State =
            (state, _) =>
            {
                state.Status = (Schema.EngineStatus)((int)state.Status).UnsetFlag((int)Schema.EngineStatus.Started);
                return state;
            };

    private static readonly Hope2Cmd<Cmd, Contract.Stop.Hope>
        _hope2Cmd =
            hope =>
                Cmd.New(hope.AggId.IDFromIdString(), hope.Payload);

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
                    Log.Debug(e.InnerAndOuter());
                    fbk.SetError(e.AsError());
                }

                return fbk;
            };

    private static readonly RaiseFuncT<Engine, Cmd>
        _raiseFunc =
            (cmd, _) =>
            {
                return new[]
                {
                    _newEvt(cmd.AggregateID, cmd.Payload, cmd.Meta)
                };
            };

    public static readonly EvtCtorT<IEvt, Contract.Stop.Payload, EventMeta>
        _newEvt =
            (id, payload, meta) => Event.New(id,
                TopicAtt.Get<IEvt>(),
                payload.ToBytes(),
                meta.ToBytes());

    public static readonly Evt2Cmd<Cmd, ChangeRpm.IEvt>
        _onZeroPowerStop =
            (evt, state) =>
            {
                var eng = (Engine)state;
                var pld = evt.GetPayload<Contract.ChangeRpm.Payload>();
                return eng.Power + pld.Delta <= 0
                    ? Cmd.New(evt.AggregateID, Contract.Stop.Payload.New())
                    : null;
            };

    public static IServiceCollection AddStopBehavior(this IServiceCollection services)
    {
        return services
            .AddStateCtor()
            .AddBaseBehavior<IEngineAggregateInfo, Engine, Cmd, IEvt>()
            .AddTransient<IAggregatePolicy, OnZeroPowerStop>()
            .AddTransient(_ => _evt2State)
            .AddTransient(_ => _specFunc)
            .AddTransient(_ => _raiseFunc)
            .AddTransient(_ => _onZeroPowerStop);
    }

    public static IServiceCollection AddStopMappers(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2State)
            .AddTransient(_ => _hope2Cmd)
            .AddTransient(_ => _evt2Fact);
    }


    [Topic(Topics.Cmd_v1)]
    public record Cmd(IID AggregateID, Contract.Stop.Payload Payload, EventMeta Meta)
        : CmdT<Contract.Stop.Payload, EventMeta>(AggregateID, Payload, Meta)
    {
        public static Cmd New(IID ID, Contract.Stop.Payload payload)
        {
            return new Cmd(ID, payload, EventMeta.New(NameAtt.Get<IEngineAggregateInfo>(), ID.Id()));
        }
    }


    [Topic(Topics.Evt_v1)]
    public interface IEvt : IEvtT<Contract.Stop.Payload>
    {
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

    [Name(OnZeroPower_v1)]
    public class OnZeroPowerStop : AggregatePolicyT<ChangeRpm.IEvt, Cmd>
    {
        public OnZeroPowerStop(
            IExchange exchange,
            Evt2Cmd<Cmd, ChangeRpm.IEvt> evt2Cmd)
            : base(exchange, evt2Cmd)
        {
        }
    }
}