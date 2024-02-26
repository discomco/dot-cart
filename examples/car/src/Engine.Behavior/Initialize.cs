using System.Collections.Immutable;
using Ardalis.GuardClauses;
using DotCart.Abstractions;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Core;
using DotCart.Abstractions.Schema;
using DotCart.Behavior;
using DotCart.Schema;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Behavior;

public static class Initialize
{
    public static readonly EvtCtorT<Contract.Initialize.Payload, MetaB> _newEvt =
        Event.New<Contract.Initialize.Payload>;

    private static readonly Evt2Fact<Contract.Initialize.Payload, MetaB> _evt2Fact =
        evt => FactT<Contract.Initialize.Payload, MetaB>.New(
            aggId: evt.AggregateId,
            payload: evt.Data.FromBytes<Contract.Initialize.Payload>()!,
            meta: evt.MetaData.FromBytes<MetaB>()!
        );

    private static readonly Fact2Msg<byte[], Contract.Initialize.Payload, MetaB>
        _fact2Msg =
            fact => fact.ToBytes();

    private static readonly Msg2Fact<Contract.Initialize.Payload, MetaB, byte[]>
        _msg2Fact =
            msg => msg.FromBytes<FactT<Contract.Initialize.Payload, MetaB>>();

    private static readonly Hope2Cmd<Contract.Initialize.Payload, MetaB>
        _hope2Cmd =
            hope =>
            {
                var id = Schema.EngineID.New();
                return Command.New<Contract.Initialize.Payload>(
                    id
                    , hope.Payload.ToBytes()
                    , MetaB.New(
                        NameAtt.Get<IEngineAggregateInfo>(),
                        id.Id()).ToBytes()
                );
            };

    private static readonly Evt2Doc<Schema.Engine, Contract.Initialize.Payload, MetaB>
        _evt2Doc =
            (state, evt) =>
            {
                if (evt == null) return state;
                if (evt.GetPayload<Contract.Initialize.Payload>() == null) return state;
                var newState = state with { };
                newState.Id = evt.AggregateId;
                newState.Details = evt.GetPayload<Contract.Initialize.Payload>().Details;
                newState.Status = Schema.Engine.Flags.Initialized;
                return newState;
            };

    private static readonly Evt2DocValidator<Schema.Engine, Contract.Initialize.Payload, MetaB>
        _evt2DocVal =
            (_, output, evt) => output.Id == evt.AggregateId
                                && output.Status.HasFlagFast(Schema.Engine.Flags.Initialized);

    private static readonly Evt2Doc<Schema.EngineList, Contract.Initialize.Payload, MetaB>
        _evt2List =
            (list, evt) =>
            {
                if (list.Items.Any(x => x.Key == evt.AggregateId))
                {
                    list.Items[evt.AggregateId].Status = Schema.Engine.Flags.Initialized;
                    return list;
                }

                var newList = list with
                {
                    Items = ImmutableDictionary<string, Schema.EngineListItem>.Empty.AddRange(list.Items)
                };
                newList.Items = list.Items.Add(evt.AggregateId, Schema.EngineListItem.New(
                    evt.AggregateId,
                    evt.GetPayload<Contract.Initialize.Payload>().Details.Name,
                    Schema.Engine.Flags.Initialized,
                    0));
                return newList;
            };

    private static readonly Evt2DocValidator<Schema.EngineList, Contract.Initialize.Payload, MetaB>
        _evt2ListVal =
            (input, output, evt) =>
            {
                var result =
                    !input.Items.Any()
                    || input.Items.All(item => item.Key != evt.AggregateId);
                result = result
                         && output.Items.Any(item => item.Key == evt.AggregateId)
                         && output.Items[evt.AggregateId].Status.HasFlagFast(Schema.Engine.Flags.Initialized);
                return result;
            };

    private static readonly GuardFuncT<Schema.Engine, Contract.Initialize.Payload, MetaB>
        _guardFunc =
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

    private static readonly RaiseFuncT<Schema.Engine, Contract.Initialize.Payload, MetaB>
        _raiseFunc =
            (cmd, _) =>
            {
                return new[]
                {
                    _newEvt(cmd.AggregateID, cmd.Data, cmd.MetaData)
                };
            };


    public static IServiceCollection AddInitializeACLFuncs(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2Fact)
            .AddTransient(_ => _hope2Cmd)
            .AddTransient(_ => _fact2Msg)
            .AddTransient(_ => _msg2Fact);
    }

    public static IServiceCollection AddInitializeProjectionFuncs(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2Doc)
            .AddTransient(_ => _evt2DocVal)
            .AddTransient(_ => _evt2List)
            .AddTransient(_ => _evt2ListVal);
    }

    public static IServiceCollection AddInitializeBehavior(this IServiceCollection services)
    {
        return services
            .AddMetaCtor()
            .AddRootDocCtors()
            .AddRootListCtors()
            .AddInitializeProjectionFuncs()
            .AddBaseBehavior<IEngineAggregateInfo, Schema.Engine, Contract.Initialize.Payload, MetaB>()
            .AddTransient(_ => _guardFunc)
            .AddTransient(_ => _raiseFunc)
            .AddTransient(_ => _newEvt);
    }

}