using Ardalis.GuardClauses;
using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behaviors;
using DotCart.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
            .AddBaseBehavior<IEngineAggregateInfo, Engine, Cmd, IEvt>()
            .AddSingleton<IAggregatePolicy, OnInitialized>()
            .AddTransient(_ => _specFunc)
            .AddTransient(_ => _raiseFunc)
            .AddTransient(_ => _evt2State)
            .AddTransient(_ => _newEvt)
            .AddTransient(_ => _initialized2Cmd);
    }

    
    public const string OnInitialized_v1 = "engine:on_initialized:change_details:v1";


    private static readonly Evt2Cmd<Cmd, Initialize.IEvt>
        _initialized2Cmd =
            evt =>
            {
                var details = evt.GetPayload<Contract.Initialize.Payload>().Details;
                return Cmd.New(
                    evt.AggregateID,
                    Contract.ChangeDetails.Payload.New(details),
                    evt.GetMeta<EventMeta>());
            };

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

    public static readonly EvtCtorT<
            IEvt, 
            Contract.ChangeDetails.Payload, 
            EventMeta>
        _newEvt =
            (id, payload, meta) => Event.New(id,
                TopicAtt.Get<IEvt>(),
                payload.ToBytes(),
                meta.ToBytes());

    private static readonly RaiseFuncT<Engine, Cmd>
        _raiseFunc =
            (cmd, _) =>
            {
                return new[]
                {
                    _newEvt(cmd.AggregateID, cmd.Payload, cmd.Meta)
                };
            };
    

    [Topic(Topics.Cmd_v1)]
    public record Cmd(IID AggregateID, Contract.ChangeDetails.Payload Payload, EventMeta Meta)
        : CmdT<Contract.ChangeDetails.Payload, EventMeta>(AggregateID,
            Payload, Meta)
    {
        public static Cmd New(IID engineId, Contract.ChangeDetails.Payload payload, EventMeta meta)
        {
            return new(engineId, payload, meta);
        }
    }

    public static class Topics
    {
        public const string Cmd_v1 = "engine:change_details:v1";
        public const string Evt_v1 = "engine:details_changed:v1";
    }

    [Topic(Topics.Evt_v1)]
    public interface IEvt : IEvtT<Contract.ChangeDetails.Payload> {}

    [Name(OnInitialized_v1)]
    public class OnInitialized : AggregatePolicyT<Initialize.IEvt, Cmd>
    {
        public OnInitialized(IExchange exchange, Evt2Cmd<Cmd, Initialize.IEvt> evt2Cmd) : base(exchange, evt2Cmd)
        {
        }
    }
}