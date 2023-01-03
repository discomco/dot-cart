using System.Collections.Immutable;
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
    public const string OnZeroPower_v1 = "engine:on_zero_power:stop:v1";

    private static readonly Evt2Fact<Contract.Stop.Payload, EventMeta>
        _evt2Fact =
            evt => FactT<Contract.Stop.Payload,EventMeta>
                .New(
                    evt.AggregateId,
                    evt.Payload,
                    evt.Meta
                );

    private static readonly Evt2Doc<Schema.Engine, Contract.Stop.Payload, EventMeta>
        _evt2Doc =
            (doc, _) =>
            {
                var newDoc = doc with { };
                newDoc.Status = newDoc.Status
                    .UnsetFlag(Schema.EngineStatus.Started)
                    .SetFlag(Schema.EngineStatus.Stopped);
                return newDoc;
            };

    private static readonly Evt2DocValidator<Schema.Engine, Contract.Stop.Payload, EventMeta>
        _evt2DocVal =
            (_, output, _)
                => output.Status.HasFlagFast(Schema.EngineStatus.Stopped)
                   && !output.Status.HasFlagFast(Schema.EngineStatus.Started);


    private static readonly Evt2Doc<Schema.EngineList, Contract.Stop.Payload, EventMeta>
        _evt2List =
            (lst, evt) =>
            {
                if (lst.Items.All(it => it.Key != evt.AggregateId))
                    return lst;
                var newList = lst with
                {
                    Items = ImmutableDictionary.CreateRange(lst.Items)
                };
                var newListItem = newList.Items[evt.AggregateId] with { };
                newListItem.Status = newListItem.Status
                    .UnsetFlag(Schema.EngineStatus.Started)
                    .SetFlag(Schema.EngineStatus.Stopped);
                newList.Items = newList.Items
                    .Remove(evt.AggregateId)
                    .Add(evt.AggregateId, newListItem);
                return newList;
            };

    private static readonly Hope2Cmd<Contract.Stop.Payload, EventMeta>
        _hope2Cmd =
            hope =>
                CmdT<Contract.Stop.Payload, EventMeta>.New(
                    hope.AggId.IDFromIdString(),
                    hope.Payload,
                    EventMeta.New(NameAtt.Get<IEngineAggregateInfo>(), hope.AggId));

    private static readonly GuardFuncT<Schema.Engine, Contract.Stop.Payload, EventMeta>
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

    private static readonly RaiseFuncT<Schema.Engine, Contract.Stop.Payload, EventMeta>
        _raiseFunc =
            (cmd, _) =>
            {
                return new[]
                {
                    _newEvt(cmd.AggregateID, cmd.Payload, cmd.Meta)
                };
            };

    public static readonly EvtCtorT<Contract.Stop.Payload, EventMeta>
        _newEvt =
            (id, payload, meta) => EvtT<Contract.Stop.Payload, EventMeta>.New(
                id.Id(),
                payload,
                meta);

    private static readonly Evt2Cmd<Contract.Stop.Payload, Contract.ChangeRpm.Payload, EventMeta>
        _shouldStopOnZeroPower =
            (evt, state) =>
            {
                var eng = (Schema.Engine)state;
                var pld = evt.GetPayload<Contract.ChangeRpm.Payload>();
                return (eng.Power + pld.Delta <= 0
                    ? CmdT<Contract.Stop.Payload, EventMeta>.New(
                        evt.AggregateID,
                        Contract.Stop.Payload.New(),
                        EventMeta.New(NameAtt.Get<IEngineAggregateInfo>(), evt.AggregateId)
                    )
                    : null)!;
            };

    private static readonly Evt2DocValidator<Schema.EngineList, Contract.Stop.Payload, EventMeta>
        _evt2ListVal =
            (_, output, evt) =>
            {
                if (output.Items.All(item => item.Key != evt.AggregateId))
                    return false;
                return output.Items[evt.AggregateId].Status.HasFlagFast(Schema.EngineStatus.Stopped)
                       && !output.Items[evt.AggregateId].Status.HasFlagFast(Schema.EngineStatus.Started);
            };

    public static IServiceCollection AddStopBehavior(this IServiceCollection services)
    {
        return services
            .AddMetaCtor()
            .AddRootDocCtors()
            .AddBaseBehavior<IEngineAggregateInfo, Schema.Engine, Contract.Stop.Payload, EventMeta>()
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
            .AddTransient(_ => _evt2Fact);
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