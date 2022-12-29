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
    public const string OnInitialized_v1 = "engine:on_initialized:change_details:v1";


    private static readonly Evt2Cmd<Cmd, Initialize.IEvt>
        _shouldChangeDetailsOnInitialized =
            (evt, _) =>
            {
                var details = evt.GetPayload<Contract.Initialize.Payload>().Details;
                return Cmd.New(
                    evt.AggregateID,
                    Contract.ChangeDetails.Payload.New(details),
                    evt.GetMeta<EventMeta>());
            };

    private static readonly Evt2Doc<Schema.Engine, IEvt>
        _evt2Doc =
            (doc, evt) =>
            {
                var newDoc = doc with { };
                newDoc.Details = evt.GetPayload<Contract.ChangeDetails.Payload>().Details;
                return newDoc;
            };

    private static readonly Evt2DocValidator<Schema.Engine, IEvt>
        _evt2DocVal =
            (input, output, evt) =>
            {
                var pld = evt.GetPayload<Contract.ChangeDetails.Payload>();
                return output.Details.Name == pld.Details.Name
                       && output.Details.Description == pld.Details.Description
                       && (input.Details.Name != output.Details.Name
                           || input.Details.Description != output.Details.Description);
            };

    private static readonly Evt2Doc<Schema.EngineList, IEvt>
        _evt2List =
            (doc, evt) =>
            {
                if (doc.Items.All(x => x.Key != evt.AggregateId))
                    return doc;
                var newDoc = doc with { };
                newDoc.Items[evt.AggregateId].Name = evt.GetPayload<Contract.ChangeDetails.Payload>().Details.Name;
                return newDoc;
            };

    private static readonly Evt2DocValidator<Schema.EngineList, IEvt>
        _evt2ListVal =
            (_, output, evt) =>
                output.Items[evt.AggregateId].Name == evt.GetPayload<Contract.ChangeDetails.Payload>().Details.Name;

    private static readonly GuardFuncT<Schema.Engine, Cmd>
        _guardFunc = (cmd, state) =>
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

    private static readonly RaiseFuncT<Schema.Engine, Cmd>
        _raiseFunc =
            (cmd, _) =>
            {
                return new[]
                {
                    _newEvt(cmd.AggregateID, cmd.Payload, cmd.Meta)
                };
            };

    public static IServiceCollection AddChangeDetailsBehavior(this IServiceCollection services)
    {
        return services
            .AddRootDocCtors()
            .AddRootListCtors()
            .AddChangeDetailsProjectionFuncs()
            .AddBaseBehavior<IEngineAggregateInfo, Schema.Engine, Cmd, IEvt>()
            .AddChoreography(_shouldChangeDetailsOnInitialized)
            .AddTransient(_ => _guardFunc)
            .AddTransient(_ => _raiseFunc)
            .AddTransient(_ => _newEvt);
    }

    public static IServiceCollection AddChangeDetailsProjectionFuncs(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2Doc)
            .AddTransient(_ => _evt2DocVal)
            .AddTransient(_ => _evt2List)
            .AddTransient(_ => _evt2ListVal);
    }


    [Topic(Topics.Cmd_v1)]
    public record Cmd(IID AggregateID, Contract.ChangeDetails.Payload Payload, EventMeta Meta)
        : CmdT<Contract.ChangeDetails.Payload, EventMeta>(AggregateID,
            Payload, Meta)
    {
        public static Cmd New(IID engineId, Contract.ChangeDetails.Payload payload, EventMeta meta)
        {
            return new Cmd(engineId, payload, meta);
        }
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

    // [Name(OnInitialized_v1)]
    // public class OnInitialized : AggregateRuleT<Initialize.IEvt, Cmd>
    // {
    //     public OnInitialized(Evt2Cmd<Cmd, Initialize.IEvt> evt2Cmd) 
    //         : base(evt2Cmd)
    //     {
    //     }
    // }
}