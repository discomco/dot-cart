using System.Collections.Immutable;
using System.Runtime.Serialization;
using Ardalis.GuardClauses;
using DotCart.Abstractions;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behavior;
using DotCart.Core;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Behavior;

public static class ChangeRpm
{
    private static readonly RaiseFuncT<Schema.Engine, Contract.ChangeRpm.Payload, MetaB>
        _raiseFunc =
            (cmd, _) =>
            {
                var res = new List<Event>();
                var rpmChanged = _newEvt(cmd.AggregateID, cmd.Data, cmd.MetaData);
                res.Add(rpmChanged);
                return res;
            };

    private static readonly GuardFuncT<Schema.Engine, Contract.ChangeRpm.Payload, MetaB>
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
                    fbk.SetError(e.AsError());
                }

                return fbk;
            };

    private static readonly Evt2Doc<Schema.Engine, Contract.ChangeRpm.Payload, MetaB>
        _evt2Doc =
            (doc, evt) =>
            {
                var newDoc = doc with { };
                newDoc.Rpm = calcRpm(newDoc.Rpm.Value, evt.GetPayload<Contract.ChangeRpm.Payload>().Delta);
                return newDoc;
            };

    private static readonly Evt2DocValidator<Schema.Engine, Contract.ChangeRpm.Payload, MetaB>
        _evt2DocVal =
            (input, output, evt) =>
            {
                var newPower = calcRpm(input.Rpm.Value, evt.GetPayload<Contract.ChangeRpm.Payload>().Delta);
                return output.Rpm.Value == newPower.Value;
            };


    private static readonly Evt2Doc<Schema.EngineList, Contract.ChangeRpm.Payload, MetaB>
        _evt2List =
            (doc, evt) =>
            {
                if (doc.Items.All(item => item.Key != evt.AggregateId))
                    return doc;
                var newItem = doc.Items[evt.AggregateId] with { };
                newItem.Power = calcRpm(newItem.Power, evt.GetPayload<Contract.ChangeRpm.Payload>().Delta).Value;
                var newDoc = doc with
                {
                    Items = ImmutableDictionary.CreateRange(doc.Items)
                };
                newDoc.Items = newDoc.Items.Remove(evt.AggregateId);
                newDoc.Items = newDoc.Items.Add(evt.AggregateId, newItem);
                return newDoc;
            };

    private static readonly Evt2DocValidator<Schema.EngineList, Contract.ChangeRpm.Payload, MetaB>
        _evt2ListVal =
            (input, output, evt) =>
            {
                if (output.Items.All(item => item.Key != evt.AggregateId))
                    return false;
                if (input.Items.All(item => item.Key != evt.AggregateId))
                    return false;
                var d = evt.GetPayload<Contract.ChangeRpm.Payload>().Delta;
                return output.Items[evt.AggregateId].Power == calcRpm(input.Items[evt.AggregateId].Power, d).Value;
            };


    private static readonly Evt2Fact<Contract.ChangeRpm.Payload, MetaB>
        _evt2Fact =
            evt => FactT<Contract.ChangeRpm.Payload, MetaB>.New(
                evt.AggregateId,
                evt.GetPayload<Contract.ChangeRpm.Payload>(),
                evt.GetMeta<MetaB>());

    private static readonly Hope2Cmd<Contract.ChangeRpm.Payload, MetaB>
        _hope2Cmd =
            hope => Command.New<Contract.ChangeRpm.Payload>(
                hope.AggId.IDFromIdString(),
                hope.Payload.ToBytes(),
                MetaB.New(NameAtt.Get<IEngineAggregateInfo>(), hope.AggId).ToBytes()
            );

    public static EvtCtorT<Contract.ChangeRpm.Payload, MetaB>
        _newEvt =
            (id, payload, meta)
                => Event.New<Contract.ChangeRpm.Payload>(
                    id,
                    payload,
                    meta
                );

    public static IServiceCollection AddChangeRpmACLFuncs(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2Doc)
            .AddTransient(_ => _evt2Fact)
            .AddTransient(_ => _hope2Cmd);
    }

    public static IServiceCollection AddChangeRpmProjectionFuncs(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2Doc)
            .AddTransient(_ => _evt2DocVal)
            .AddTransient(_ => _evt2List)
            .AddTransient(_ => _evt2ListVal);
    }

    public static IServiceCollection AddChangeRpmBehavior(this IServiceCollection services)
    {
        return services
            .AddMetaCtor()
            .AddRootDocCtors()
            .AddBaseBehavior<IEngineAggregateInfo, Schema.Engine, Contract.ChangeRpm.Payload, MetaB>()
            .AddChangeRpmProjectionFuncs()
            .AddTransient(_ => _guardFunc)
            .AddTransient(_ => _raiseFunc);
    }

    private static Schema.Rpm calcRpm(int original, int delta)
    {
        var newPower = original + delta;
        if (newPower <= 0)
            newPower = 0;
        return Schema.Rpm.New(newPower);
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
}