using System.Runtime.Serialization;
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

public static class Stop
{
    public const string OnZeroPower_v1 = "engine:on_zero_power:stop:v1";

    private static readonly Evt2Fact<Contract.Stop.Payload, MetaB>
        _evt2Fact =
            evt => FactT<Contract.Stop.Payload, MetaB>
                .New(
                    evt.AggregateId,
                    evt.GetPayload<Contract.Stop.Payload>(),
                    evt.GetMeta<MetaB>()
                );

    private static readonly Evt2Doc<Schema.Engine, Contract.Stop.Payload, MetaB>
        _evt2Doc =
            (doc, _) =>
            {
                var newDoc = doc with { };
                newDoc.Status = newDoc.Status
                    .UnsetFlag(Schema.Engine.Flags.Started)
                    .SetFlag(Schema.Engine.Flags.Stopped);
                return newDoc;
            };

    private static readonly Evt2DocValidator<Schema.Engine, Contract.Stop.Payload, MetaB>
        _evt2DocVal =
            (_, output, _)
                => output.Status.HasFlagFast(Schema.Engine.Flags.Stopped)
                   && !output.Status.HasFlagFast(Schema.Engine.Flags.Started);


    private static readonly Evt2Doc<Schema.EngineList, Contract.Stop.Payload, MetaB>
        _evt2List =
            (lst, evt) =>
            {
                if (lst.Items.All(it => it.Key != evt.AggregateId))
                    return lst;
                // var newList = lst with
                // {
                //     Items = ImmutableDictionary.CreateRange(lst.Items)
                // };
                var newList = lst;
                var newListItem = newList.Items[evt.AggregateId] with { };
                newListItem.Status = newListItem.Status
                    .UnsetFlag(Schema.Engine.Flags.Started)
                    .SetFlag(Schema.Engine.Flags.Stopped);
                newList.Items = newList.Items
                    .Remove(evt.AggregateId)
                    .Add(evt.AggregateId, newListItem);
                return newList;
            };

    private static readonly Hope2Cmd<Contract.Stop.Payload, MetaB>
        _hope2Cmd =
            hope =>
                Command.New<Contract.Stop.Payload>(
                    hope.AggId.IDFromIdString(),
                    hope.Payload.ToBytes(),
                    MetaB.New(NameAtt.Get<IEngineAggregateInfo>(), hope.AggId).ToBytes());

    private static readonly GuardFuncT<Schema.Engine, Contract.Stop.Payload, MetaB>
        _guardFunc =
            (cmd, state) =>
            {
                var fbk = Feedback.New(cmd.AggregateID.Id());
                try
                {
                    Guard.Against
                        .EngineNotInitialized(state)
                        .EngineNotStarted(state)
                        .EngineStopped(state);
                }
                catch (Exception e)
                {
                    Log.Debug(e.InnerAndOuter());
                    fbk.SetError(e.AsError());
                }

                return fbk;
            };

    private static readonly RaiseFuncT<Schema.Engine, Contract.Stop.Payload, MetaB>
        _raiseFunc =
            (cmd, _) =>
            {
                return new[]
                {
                    _newEvt(cmd.AggregateID, cmd.Data, cmd.MetaData)
                };
            };

    public static readonly EvtCtorT<Contract.Stop.Payload, MetaB>
        _newEvt =
            Event.New<Contract.Stop.Payload>;

    public static readonly Fact2Msg<byte[], Contract.Stop.Payload, MetaB>
        _fact2Msg =
            fact => fact.ToBytes();

    private static readonly Msg2Fact<Contract.Stop.Payload, MetaB, byte[]>
        _msg2Fact =
            msg => msg.FromBytes<FactT<Contract.Stop.Payload, MetaB>>();

    private static readonly Evt2Cmd<Contract.Stop.Payload, Contract.ChangeRpm.Payload, MetaB>
        _shouldStopOnZeroPower =
            (evt, state) =>
            {
                var eng = (Schema.Engine)state;
                var pld = evt.GetPayload<Contract.ChangeRpm.Payload>();
                return (eng.Rpm.Value + pld.Delta <= 0
                    ? Command.New<Contract.Stop.Payload>(
                        evt.AggregateID,
                        Contract.Stop.Payload.New().ToBytes(),
                        MetaB.New(NameAtt.Get<IEngineAggregateInfo>(), evt.AggregateId).ToBytes()
                    )
                    : null)!;
            };

    private static readonly Evt2DocValidator<Schema.EngineList, Contract.Stop.Payload, MetaB>
        _evt2ListVal =
            (_, output, evt) =>
            {
                if (output.Items.All(item => item.Key != evt.AggregateId))
                    return false;
                return output.Items[evt.AggregateId].Status.HasFlagFast(Schema.Engine.Flags.Stopped)
                       && !output.Items[evt.AggregateId].Status.HasFlagFast(Schema.Engine.Flags.Started);
            };

    public static IServiceCollection AddStopBehavior(this IServiceCollection services)
    {
        return services
            .AddMetaCtor()
            .AddRootDocCtors()
            .AddBaseBehavior<IEngineAggregateInfo, Schema.Engine, Contract.Stop.Payload, MetaB>()
            .AddChoreography(_shouldStopOnZeroPower)
            .AddStopProjectionFuncs()
            .AddTransient(_ => _newEvt)
            .AddTransient(_ => _guardFunc)
            .AddTransient(_ => _raiseFunc);
    }

    public static IServiceCollection AddStopProjectionFuncs(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2List)
            .AddTransient(_ => _evt2ListVal)
            .AddTransient(_ => _evt2Doc)
            .AddTransient(_ => _evt2DocVal);
    }

    public static IServiceCollection AddStopACLFuncs(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2Doc)
            .AddTransient(_ => _hope2Cmd)
            .AddTransient(_ => _evt2Fact)
            .AddTransient(_ => _fact2Msg)
            .AddTransient(_ => _msg2Fact);
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
    }
}