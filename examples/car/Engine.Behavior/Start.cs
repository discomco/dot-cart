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
            .AddTransient<IAggregatePolicy, StartOnInitializedPolicy>()
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

    private static Evt2Cmd<Cmd, Initialize.IEvt> _evt2Cmd =>
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
                    NewEvt(cmd.AggregateID, cmd.Payload, cmd.Meta)
                };
            };


    public static Event NewEvt(IID aggregateID, Contract.Start.Payload payload, EventMeta meta)
    {
        return Event.New(aggregateID, 
            TopicAtt.Get<IEvt>(), 
            payload.ToBytes(), 
            meta.ToBytes());
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


    public const string EngineStartsOnInitializedPolicy = "engine:starts_on_nitialized:policy";

    [Name(EngineStartsOnInitializedPolicy)]
    public class StartOnInitializedPolicy : AggregatePolicyT<Initialize.IEvt, Cmd>
    {
        public StartOnInitializedPolicy(
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

    // public record EvtMeta(string AggregateType, string AggregateId)
    //     : EventMeta(AggregateType, AggregateId);

    
    
    [Topic(Topics.Evt_v1)]
    public interface IEvt : IEvtT<Contract.Start.Payload>
    {}
    

    // [Topic(Topics.Evt_v1)]
    // public record Evt(IID AggregateID,
    //     Contract.Start.Payload Payload,
    //     EvtMeta Meta) : EvtT<Contract.Start.Payload, EvtMeta>(AggregateID,
    //     TopicAtt.Get<Evt>(),
    //     Payload,
    //     Meta)
    // {
    //     public static Evt New(IID aggregateID, Contract.Start.Payload payload)
    //     {
    //         var meta = new EvtMeta(NameAtt.Get<IEngineAggregateInfo>(), aggregateID.Id());
    //         return new Evt(aggregateID, payload, meta);
    //     }
    // }
}