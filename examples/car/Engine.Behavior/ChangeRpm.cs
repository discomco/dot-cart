using System.Collections.Immutable;
using System.Runtime.Serialization;
using Ardalis.GuardClauses;
using DotCart.Abstractions;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behaviors;
using DotCart.Core;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Behavior;

public static class ChangeRpm
{
    private static readonly RaiseFuncT<Schema.Engine, Contract.ChangeRpm.Payload, EventMeta>
        _raiseFunc =
            (cmd, _) =>
            {
                var res = new List<Event>();
                var rpmChanged = _newEvt(cmd.AggregateID, cmd.Data, cmd.MetaData);
                res.Add(rpmChanged);
                return res;
            };

    private static readonly GuardFuncT<Schema.Engine, Contract.ChangeRpm.Payload, EventMeta>
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

    private static readonly Evt2Doc<Schema.Engine, Contract.ChangeRpm.Payload, EventMeta>
        _evt2Doc =
            (doc, evt) =>
            {
                var newDoc = doc with { };
                var newPower = calcPower(newDoc.Power, evt.GetPayload<Contract.ChangeRpm.Payload>().Delta);
                newDoc.Power = newPower;
                return newDoc;
            };

    private static readonly Evt2DocValidator<Schema.Engine, Contract.ChangeRpm.Payload, EventMeta>
        _evt2DocVal =
            (input, output, evt) =>
            {
                var newPower = calcPower(input.Power, evt.GetPayload<Contract.ChangeRpm.Payload>().Delta);
                return output.Power == newPower;
            };


    private static readonly Evt2Doc<Schema.EngineList, Contract.ChangeRpm.Payload, EventMeta>
        _evt2List =
            (doc, evt) =>
            {
                if (doc.Items.All(item => item.Key != evt.AggregateId))
                    return doc;
                var newItem = doc.Items[evt.AggregateId] with { };
                newItem.Power = calcPower(newItem.Power, evt.GetPayload<Contract.ChangeRpm.Payload>().Delta);
                var newDoc = doc with
                {
                    Items = ImmutableDictionary.CreateRange(doc.Items)
                };
                newDoc.Items = newDoc.Items.Remove(evt.AggregateId);
                newDoc.Items = newDoc.Items.Add(evt.AggregateId, newItem);
                return newDoc;
            };

    private static readonly Evt2DocValidator<Schema.EngineList, Contract.ChangeRpm.Payload, EventMeta>
        _evt2ListVal =
            (input, output, evt) =>
            {
                if (output.Items.All(item => item.Key != evt.AggregateId))
                    return false;
                if (input.Items.All(item => item.Key != evt.AggregateId))
                    return false;
                var d = evt.GetPayload<Contract.ChangeRpm.Payload>().Delta;
                return output.Items[evt.AggregateId].Power == calcPower(input.Items[evt.AggregateId].Power, d);
            };


    private static readonly Evt2Fact<Contract.ChangeRpm.Payload, EventMeta>
        _evt2Fact =
            evt => FactT<Contract.ChangeRpm.Payload, EventMeta>.New(
                evt.AggregateId,
                evt.GetPayload<Contract.ChangeRpm.Payload>(),
                evt.GetMeta<EventMeta>());

    private static readonly Hope2Cmd<Contract.ChangeRpm.Payload, EventMeta>
        _hope2Cmd =
            hope => Command.New<Contract.ChangeRpm.Payload>(
                hope.AggId.IDFromIdString(),
                hope.Payload.ToBytes(),
                EventMeta.New(NameAtt.Get<IEngineAggregateInfo>(), hope.AggId).ToBytes()
            );

    public static EvtCtorT<Contract.ChangeRpm.Payload, EventMeta>
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
            .AddBaseBehavior<IEngineAggregateInfo, Schema.Engine, Contract.ChangeRpm.Payload, EventMeta>()
            .AddChangeRpmProjectionFuncs()
            .AddTransient(_ => _guardFunc)
            .AddTransient(_ => _raiseFunc);
    }

    private static int calcPower(int power, int delta)
    {
        var newPower = power + delta;
        if (newPower <= 0)
            newPower = 0;
        return newPower;
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