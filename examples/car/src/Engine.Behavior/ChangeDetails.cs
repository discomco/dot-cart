using Ardalis.GuardClauses;
using DotCart.Abstractions;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Core;
using DotCart.Abstractions.Schema;
using DotCart.Behavior;
using DotCart.Core;
using DotCart.Schema;
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


    private static readonly Evt2Cmd<Contract.ChangeDetails.Payload, Contract.Initialize.Payload, MetaB>
        _shouldChangeDetailsOnInitialized =
            (evt, _) =>
            {
                var details = evt.GetPayload<Contract.Initialize.Payload>().Details;
                return Command.New<Contract.ChangeDetails.Payload>(
                    evt.AggregateID,
                    Contract.ChangeDetails.Payload.New(details).ToBytes(),
                    evt.MetaData);
            };

    private static readonly Evt2Doc<Schema.Engine, Contract.ChangeDetails.Payload, MetaB>
        _evt2Doc =
            (doc, evt) =>
            {
                var newDoc = doc with { };
                newDoc.Details = evt.GetPayload<Contract.ChangeDetails.Payload>().Details;
                return newDoc;
            };

    private static readonly Evt2DocValidator<Schema.Engine, Contract.ChangeDetails.Payload, MetaB>
        _evt2DocVal =
            (input, output, evt) =>
            {
                var pld = evt.GetPayload<Contract.ChangeDetails.Payload>();
                return output.Details.Name == pld.Details.Name
                       && output.Details.Description == pld.Details.Description
                       && (input.Details.Name != output.Details.Name
                           || input.Details.Description != output.Details.Description);
            };

    private static readonly Evt2Doc<Schema.EngineList, Contract.ChangeDetails.Payload, MetaB>
        _evt2List =
            (doc, evt) =>
            {
                if (doc.Items.All(x => x.Key != evt.AggregateId))
                    return doc;
                var newDoc = doc with { };
                newDoc.Items[evt.AggregateId].Name = evt.GetPayload<Contract.ChangeDetails.Payload>().Details.Name;
                return newDoc;
            };

    private static readonly Evt2DocValidator<Schema.EngineList, Contract.ChangeDetails.Payload, MetaB>
        _evt2ListVal =
            (_, output, evt) =>
                output.Items[evt.AggregateId].Name == evt.GetPayload<Contract.ChangeDetails.Payload>().Details.Name;

    private static readonly GuardFuncT<Schema.Engine, Contract.ChangeDetails.Payload, MetaB>
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
                Log.Error($"{AppErrors.Error(e.InnerAndOuter())}");
            }

            return fbk;
        };

    public static readonly EvtCtorT<Contract.ChangeDetails.Payload, MetaB>
        _newEvt =
            (id, payload, meta) => Event.New<Contract.ChangeDetails.Payload>(
                id,
                payload,
                meta);

    private static readonly RaiseFuncT<Schema.Engine, Contract.ChangeDetails.Payload, MetaB>
        _raiseFunc =
            (cmd, _) =>
            {
                return new[]
                {
                    _newEvt(cmd.AggregateID, cmd.Data, cmd.MetaData)
                };
            };


    private static readonly Fact2Msg<byte[], Contract.ChangeDetails.Payload, MetaB>
        _fact2Msg =
            fact => fact.ToBytes();

    private static readonly Msg2Fact<Contract.ChangeDetails.Payload, MetaB, byte[]>
        _msg2Fact =
            msg => msg.FromBytes<FactT<Contract.ChangeDetails.Payload, MetaB>>();

    private static readonly Hope2Cmd<Contract.ChangeDetails.Payload, MetaB>
        _hope2Cmd =
            hope => Command.New<Contract.ChangeDetails.Payload>(
                hope.AggId.IDFromIdString(),
                hope.Payload.ToBytes(),
                MetaB.New(
                    NameAtt.Get<IEngineAggregateInfo>(),
                    hope.AggId
                ).ToBytes()
            );

    private static readonly Evt2Fact<Contract.ChangeDetails.Payload, MetaB>
        _evt2Fact =
            evt => FactT<Contract.ChangeDetails.Payload, MetaB>.New(
                evt.AggregateId,
                evt.GetPayload<Contract.ChangeDetails.Payload>(),
                evt.GetMeta<MetaB>()
            );

    public static IServiceCollection AddChangeDetailsACLFuncs(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2Fact)
            .AddTransient(_ => _hope2Cmd)
            .AddTransient(_ => _fact2Msg)
            .AddTransient(_ => _msg2Fact);
    }


    public static IServiceCollection AddChangeDetailsBehavior(this IServiceCollection services)
    {
        return services
            .AddMetaCtor()
            .AddRootDocCtors()
            .AddRootListCtors()
            .AddChangeDetailsProjectionFuncs()
            .AddBaseBehavior<IEngineAggregateInfo, Schema.Engine, Contract.ChangeDetails.Payload, MetaB>()
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
}