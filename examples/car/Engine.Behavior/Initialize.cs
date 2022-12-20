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

public static partial class Inject
{
}

public static class Initialize
{
    public static readonly EvtCtorT<IEvt, Contract.Initialize.Payload, EventMeta>
        _newEvt =
            (id, payload, meta) => Event.New(id,
                TopicAtt.Get<IEvt>(),
                payload.ToBytes(),
                meta.ToBytes()
            );

    private static readonly Evt2Fact<
            Contract.Initialize.Fact,
            IEvt>
        _evt2Fact =
            evt => Contract.Initialize.Fact.New(evt.AggregateId, evt.GetPayload<Contract.Initialize.Payload>());

    private static readonly Hope2Cmd<
            Cmd,
            Contract.Initialize.Hope>
        _hope2Cmd =
            hope => Cmd.New(Schema.EngineID.New(), hope.Payload);

    private static readonly Evt2State<
            Engine,
            IEvt>
        _evt2State =
            (state, evt) =>
            {
                if (evt == null)
                    return state;
                if (evt.GetPayload<Contract.Initialize.Payload>() == null) return state;
                state.Id = evt.AggregateId;
                state.Details = evt.GetPayload<Contract.Initialize.Payload>().Details;
                state.Status = Schema.EngineStatus.Initialized;
                return state;
            };

    private static readonly SpecFuncT<
            Engine,
            Cmd>
        _specFunc =
            (cmd, state) =>
            {
                var fbk = Feedback.New(cmd.AggregateID.Id());
                try
                {
                    Guard.Against.EngineInitialized(state);
                }
                catch (Exception e)
                {
                    fbk.SetError(e.AsError());
                    Console.WriteLine(e);
                }

                return fbk;
            };


    // public static Event NewEvt(
    //     IID aggregateID,
    //     Contract.Initialize.Payload payload,
    //     EventMeta meta)
    // {
    //     return Event.New(aggregateID,
    //         TopicAtt.Get<IEvt>(),
    //         payload.ToBytes(),
    //         meta.ToBytes()
    //     );
    // }

    private static readonly RaiseFuncT<Engine, Cmd>
        _raiseFunc =
            (cmd, _) =>
            {
                return new[]
                {
                    _newEvt(cmd.AggregateID, cmd.Payload, cmd.Meta)
                };
            };

    public static IServiceCollection AddInitializeMappers(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2Fact)
            .AddTransient(_ => _evt2State)
            .AddTransient(_ => _hope2Cmd);
    }

    public static IServiceCollection AddInitializeBehavior(this IServiceCollection services)
    {
        return services
            .AddStateCtor()
            .AddBaseBehavior<IEngineAggregateInfo, Engine, Cmd, IEvt>()
            .AddTransient(_ => _evt2State)
            .AddTransient(_ => _specFunc)
            .AddTransient(_ => _raiseFunc)
            .AddTransient(_ => _newEvt);
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

    // public record EvtMeta(string AggregateType, string AggregateId)
    //     : EventMeta(AggregateType, AggregateId);


    [Topic(Topics.Evt_v1)]
    public interface IEvt : IEvtT<Contract.Initialize.Payload>
    {
    }


    // public record Evt(IID AggregateID,
    //     Contract.Initialize.Payload Payload,
    //     EvtMeta Meta) : EvtT<
    //     Contract.Initialize.Payload,
    //     EvtMeta>(AggregateID,
    //     TopicAtt.Get<Evt>(),
    //     Payload,
    //     Meta)
    // {
    //     public static Evt New(IID aggregateID, Contract.Initialize.Payload payload)
    //     {
    //         var meta = new EvtMeta(NameAtt.Get<IEngineAggregateInfo>(), aggregateID.Id());
    //         return new Evt(aggregateID, payload, meta);
    //     }
    // }


    [Topic(Topics.Cmd_v1)]
    public record Cmd(IID AggregateID, Contract.Initialize.Payload Payload, EventMeta Meta)
        : CmdT<Contract.Initialize.Payload, EventMeta>(AggregateID, Payload, Meta)
    {
        public static Cmd New(IID aggID, Contract.Initialize.Payload payload)
        {
            return new Cmd(
                aggID, payload,
                EventMeta.New(
                    NameAtt.Get<IEngineAggregateInfo>(),
                    aggID.Id()
                ));
        }
    }

    public static class Topics
    {
        public const string Cmd_v1 = "engine:initialize:v1";
        public const string Evt_v1 = "engine:initialized:v1";
    }
}