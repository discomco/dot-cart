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
    public static IServiceCollection AddStopBehavior(this IServiceCollection services)
    {
        return services
            .AddStateCtor()
            .AddBaseBehavior<IEngineAggregateInfo, Engine, Cmd, IEvt>()
            .AddTransient(_ => _evt2State)
            .AddTransient(_ => _specFunc)
            .AddTransient(_ => _raiseFunc);
    }

    public static IServiceCollection AddStopMappers(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2State)
            .AddTransient(_ => _hope2Cmd);
    }


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
                    NewEvt(cmd.AggregateID, cmd.Payload, cmd.Meta)
                };
            };


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
    public interface IEvt: IEvtT<Contract.Stop.Payload>
    {}

    public static Event NewEvt(IID aggregateID, Contract.Stop.Payload payload, EventMeta meta)
    {
        return Event.New(aggregateID, 
            TopicAtt.Get<IEvt>(), 
            payload.ToBytes(), 
            meta.ToBytes());
    }
    
    // public record Evt(IID AggregateID,
    //         Contract.Stop.Payload Payload,
    //         EvtMeta Meta)
    //     : EvtT<Contract.Stop.Payload, EvtMeta>(
    //         AggregateID,
    //         TopicAtt.Get<Evt>(),
    //         Payload,
    //         Meta)
    // {
    //     public static Evt New(IID AggregateID, Contract.Stop.Payload payload)
    //     {
    //         var meta = new EvtMeta(NameAtt.Get<IEngineAggregateInfo>(), AggregateID.Id());
    //         return new Evt(AggregateID, payload, meta);
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
    }

    public static class Topics
    {
        public const string Evt_v1 = "engine:stopped:v1";
        public const string Cmd_v1 = "engine:stop:v1";
    }
}