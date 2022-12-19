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

public static partial class Inject
{
}

public static class ChangeDetails
{
    public static IServiceCollection AddChangeDetailsBehavior(this IServiceCollection services)
    {
        return services
            .AddStateCtor()
            .AddBaseBehavior<IEngineAggregateInfo, Engine, ChangeDetails.Cmd, ChangeDetails.IEvt>()
            .AddTransient(_ => _specFunc)
            .AddTransient(_ => _raiseFunc)
            .AddTransient(_ => _evt2State);
    }


    private static readonly Evt2State<Engine, IEvt>
        _evt2State =
            (state, evt) =>
            {
                state.Details = evt.GetPayload<Contract.ChangeDetails.Payload>().Details;
                return state;
            };

    private static readonly SpecFuncT<Engine, Cmd>
        _specFunc = (cmd, state) =>
        {
            var fbk = Feedback.New(cmd.AggregateID.Id());
            try
            {
                Guard.Against.EngineNotInitialized(state);
            }
            catch (Exception e)
            {
                fbk.SetError(e.AsError());
                Log.Error($"{AppErrors.Error} - {e.InnerAndOuter()}");
            }

            return fbk;
        };


    public static Event NewEvt(IID aggregateID, Contract.ChangeDetails.Payload payload)
    {
        return Event.New(aggregateID,
            TopicAtt.Get<IEvt>(),
            payload.ToBytes(),
            EventMeta.New(NameAtt.Get<IEngineAggregateInfo>(), aggregateID.Id()).ToBytes()
        );
    }

    private static readonly RaiseFuncT<Engine, Cmd>
        _raiseFunc =
            (cmd, _) =>
            {
                return new[]
                {
                    NewEvt(cmd.AggregateID, cmd.Payload)
                };
            };

    [Topic(Topics.Cmd_v1)]
    public record Cmd(IID AggregateID, Contract.ChangeDetails.Payload Payload, EventMeta Meta)
        : CmdT<Contract.ChangeDetails.Payload, EventMeta>(AggregateID,
            Payload, Meta)
    {
        public static Cmd New(Schema.EngineID engineId, Contract.ChangeDetails.Payload payload, EventMeta meta)
            => new(engineId, payload, meta);
    }

    public static class Topics
    {
        public const string Cmd_v1 = "engine:change_details:v1";
        public const string Evt_v1 = "engine:details_changed:v1";
    }

    [Topic(Topics.Evt_v1)]
    public interface IEvt : IEvtT<Contract.ChangeDetails.Payload>
    {
    }

    /// TODO: Remove this
    // public record Evt(IID AggregateID, 
    //     Contract.ChangeDetails.Payload Payload,
    //     EventMeta Meta) 
    //     : EvtT<Contract.ChangeDetails.Payload, EventMeta>(
    //         AggregateID, 
    //         TopicAtt.Get<Evt>(), 
    //         Payload, 
    //         Meta)
    // {
    //     
    //     public static Evt New(
    //         IID engineID,
    //         Contract.ChangeDetails.Payload payload)
    //     {
    //         var meta = EventMeta.New(
    //             NameAtt.Get<IEngineAggregateInfo>(), 
    //             engineID.Id());
    //         return new Evt(engineID, payload, meta);
    //     }
    // }
}