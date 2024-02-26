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

public static class Start
{
    public const string OnInitialized_v1 = "engine:on_initialized:start:v1";


    private static readonly Evt2Fact<Contract.Start.Payload, MetaB>
        _evt2Fact =
            evt =>
                FactT<Contract.Start.Payload, MetaB>.New(
                    evt.AggregateId,
                    evt.GetPayload<Contract.Start.Payload>(),
                    evt.GetMeta<MetaB>()
                );


    private static readonly Fact2Msg<byte[], Contract.Start.Payload, MetaB>
        _fact2Msg =
            fact => fact.ToBytes();

    private static readonly Msg2Fact<Contract.Start.Payload, MetaB, byte[]>
        _msg2Fact =
            msg => msg.FromBytes<FactT<Contract.Start.Payload, MetaB>>();

    private static readonly Hope2Cmd<Contract.Start.Payload, MetaB>
        _hope2Cmd =
            hope =>
                Command.New<Contract.Start.Payload>(
                    hope.AggId.IDFromIdString(),
                    hope.Payload.ToBytes(),
                    MetaB.New(NameAtt.Get<IEngineAggregateInfo>(), hope.AggId).ToBytes()
                );


    private static readonly Evt2Doc<Schema.Engine, Contract.Start.Payload, MetaB>
        _evt2Doc =
            (doc, _) =>
            {
                var newDoc = doc with { };
                newDoc.Status = newDoc.Status
                    .UnsetFlag(Schema.Engine.Flags.Stopped)
                    .SetFlag(Schema.Engine.Flags.Started);
                return newDoc;
            };

    private static readonly Evt2DocValidator<Schema.Engine, Contract.Start.Payload, MetaB>
        _evt2DocVal =
            (input, output, _)
                => !input.Status.HasFlagFast(Schema.Engine.Flags.Started)
                   && output.Status.HasFlagFast(Schema.Engine.Flags.Started)
                   && !output.Status.HasFlagFast(Schema.Engine.Flags.Stopped);

    private static readonly Evt2Doc<Schema.EngineList, Contract.Start.Payload, MetaB>
        _evt2List =
            (lst, evt) =>
            {
                if (lst.Items.All(it => it.Key != evt.AggregateId))
                    return lst;
                var newLst = lst with
                {
                    Items = ImmutableDictionary.CreateRange(lst.Items)
                };
                newLst.Items[evt.AggregateId].Status
                    = newLst.Items[evt.AggregateId].Status.UnsetFlag(Schema.Engine.Flags.Stopped);
                newLst.Items[evt.AggregateId].Status
                    = newLst.Items[evt.AggregateId].Status.SetFlag(Schema.Engine.Flags.Started);
                return newLst;
            };

    private static readonly Evt2DocValidator<Schema.EngineList, Contract.Start.Payload, MetaB>
        _evt2ListVal =
            (input, output, evt) =>
            {
                return output.Items.Any(it => it.Key == evt.AggregateId)
                       && output.Items[evt.AggregateId].Status.HasFlagFast(Schema.Engine.Flags.Started)
                       && !output.Items[evt.AggregateId].Status.HasFlagFast(Schema.Engine.Flags.Stopped);
            };


    private static readonly GuardFuncT<Schema.Engine, Contract.Start.Payload, MetaB>
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

    private static readonly RaiseFuncT<Schema.Engine, Contract.Start.Payload, MetaB>
        _raiseFunc =
            (cmd, _) =>
            {
                return new[]
                {
                    _newEvt(cmd.AggregateID, cmd.Data, cmd.MetaData)
                };
            };


    public static readonly EvtCtorT<Contract.Start.Payload, MetaB>
        _newEvt =
            Event.New<Contract.Start.Payload>;

    private static readonly Evt2Cmd<Contract.Start.Payload, Contract.Initialize.Payload, MetaB>
        _shouldStartOnInitialized =
            (evt, _) =>
                Command.New<Contract.Start.Payload>(
                    evt.AggregateId.IDFromIdString(),
                    Contract.Start.Payload.New.ToBytes(),
                    MetaB.New(NameAtt.Get<IEngineAggregateInfo>(), evt.AggregateId).ToBytes());

    public static IServiceCollection AddStartACLFuncs(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2Fact)
            .AddTransient(_ => _evt2Doc)
            .AddTransient(_ => _hope2Cmd)
            .AddTransient(_ => _fact2Msg)
            .AddTransient(_ => _msg2Fact);
    }

    public static IServiceCollection AddStartBehavior(this IServiceCollection services)
    {
        return services
            .AddMetaCtor()
            .AddRootDocCtors()
            .AddBaseBehavior<IEngineAggregateInfo, Schema.Engine, Contract.Start.Payload, MetaB>()
            .AddChoreography(_shouldStartOnInitialized)
            .AddTransient(_ => _evt2Doc)
            .AddTransient(_ => _specFunc)
            .AddTransient(_ => _raiseFunc);
    }

    public static IServiceCollection AddStartProjectionFuncs(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2Doc)
            .AddTransient(_ => _evt2DocVal)
            .AddTransient(_ => _evt2List)
            .AddTransient(_ => _evt2ListVal);
    }



}