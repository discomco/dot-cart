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
    public static IServiceCollection AddStartMappers(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2Fact)
            .AddTransient(_ => _evt2State)
            .AddTransient(_ => _hope2Cmd);
    }

    public static IServiceCollection AddStartBehavior(this IServiceCollection services)
    {
        return services
            .AddStateCtor()
            .AddBaseBehavior<IEngineAggregateInfo, Engine, Cmd, IEvt>()
            .AddTransient<IAggregatePolicy, OnInitialized>()
            .AddTransient(_ => _evt2Cmd)
            .AddTransient(_ => _evt2State)
            .AddTransient(_ => _specFunc)
            .AddTransient(_ => _raiseFunc);
    }


    public static class Topics
    {
        public const string Cmd_v1 = "engine:start:v1";
        public const string Evt_v1 = "engine:started:v1";
    }


    private static readonly Evt2Fact<Contract.Start.Fact, IEvt>
        _evt2Fact =
            evt =>
                Contract.Start.Fact.New(evt.AggregateId, evt.GetPayload<Contract.Start.Payload>());

    private static readonly Hope2Cmd<Cmd, Contract.Start.Hope>
        _hope2Cmd =
            hope =>
                Cmd.New(hope.AggId.IDFromIdString(), hope.Payload);

    private static readonly Evt2State<Engine, IEvt>
        _evt2State =
            (state, _) =>
            {
                state.Status = state.Status.SetFlag(Schema.EngineStatus.Started);
                return state;
            };

    public static Evt2Cmd<Cmd, Initialize.IEvt> _evt2Cmd =>
        evt =>
            Cmd.New(evt.AggregateId.IDFromIdString(), Contract.Start.Payload.New);

    private static readonly SpecFuncT<Engine, Cmd>
        _specFunc =
            (_, state) =>
            {
                var fbk = Feedback.Empty;
                try
                {
                    Guard.Against.EngineNotInitialized(state);
                }
                catch (Exception e)
                {
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


    public static readonly EvtCtorT<IEvt, Contract.Start.Payload, EventMeta>
        _newEvt = 
            (id, payload, meta) => Event.New(id, 
                TopicAtt.Get<IEvt>(), 
                payload.ToBytes(), 
                meta.ToBytes());  

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


    public const string OnInitialized_v1 = "engine:on_initialized:start:v1";

    [Name(OnInitialized_v1)]
    public class OnInitialized : AggregatePolicyT<Initialize.IEvt, Cmd>
    {
        public OnInitialized(
            IExchange exchange,
            Evt2Cmd<Cmd, Initialize.IEvt> evt2Cmd)
            : base(exchange, evt2Cmd)
        {
        }
    }

    [Topic(Topics.Cmd_v1)]
    public record Cmd(IID AggregateID, Contract.Start.Payload Payload, EventMeta Meta)
        : CmdT<Contract.Start.Payload, EventMeta>(AggregateID, Payload, Meta)
    {
        public static Cmd New(IID aggregateID, Contract.Start.Payload payload)
        {
            return new Cmd(
                aggregateID, 
                payload, 
                EventMeta.New(
                    NameAtt.Get<IEngineAggregateInfo>(), 
                    aggregateID.Id()));
        }
    }

   
    [Topic(Topics.Evt_v1)]
    public interface IEvt : IEvtT<Contract.Start.Payload>
    {}
    

 
}