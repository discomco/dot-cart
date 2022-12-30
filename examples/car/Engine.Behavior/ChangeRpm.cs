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

namespace Engine.Behavior;

public static class ChangeRpm
{
    private static readonly RaiseFuncT<Schema.Engine, Cmd>
        _raiseFunc =
            (cmd, state) =>
            {
                var res = new List<Event>();
                var rpmChanged = _newEvt(cmd.AggregateID, cmd.Payload, cmd.Meta);
                res.Add(rpmChanged);
                return res;
            };

    private static readonly GuardFuncT<Schema.Engine, Cmd>
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

    private static readonly Evt2Doc<Schema.Engine, IEvt>
        _evt2Doc =
            (doc, evt) =>
            {
                var newDoc = doc with { };
                newDoc.Power += evt.GetPayload<Contract.ChangeRpm.Payload>().Delta;
                return newDoc;
            };

    private static readonly Evt2DocValidator<Schema.Engine, IEvt>
        _evt2DocVal =
            (input, output, evt)
                => output.Power == input.Power + evt.GetPayload<Contract.ChangeRpm.Payload>().Delta;


    private static readonly Evt2Doc<Schema.EngineList, IEvt>
        _evt2List =
            (doc, evt) =>
            {
                if (doc.Items.All(item => item.Key != evt.AggregateId))
                    return doc;
                var newItem = doc.Items[evt.AggregateId] with { };
                newItem.Power += evt.GetPayload<Contract.ChangeRpm.Payload>().Delta;
                var newDoc = doc with
                {
                    Items = ImmutableDictionary.CreateRange(doc.Items)
                };
                newDoc.Items = newDoc.Items.Remove(evt.AggregateId);
                newDoc.Items = newDoc.Items.Add(evt.AggregateId, newItem);
                return newDoc;
            };

    private static readonly Evt2DocValidator<Schema.EngineList, IEvt>
        _evt2ListVal =
            (input, output, evt) =>
            {
                if (output.Items.All(item => item.Key != evt.AggregateId))
                    return false;
                if (input.Items.All(item => item.Key != evt.AggregateId))
                    return false;
                var d = evt.GetPayload<Contract.ChangeRpm.Payload>().Delta;
                return output.Items[evt.AggregateId].Power - input.Items[evt.AggregateId].Power == d;
            };


    private static readonly Evt2Fact<Contract.ChangeRpm.Fact, IEvt>
        _evt2Fact =
            evt => Contract.ChangeRpm.Fact.New(
                evt.AggregateId,
                evt.GetPayload<Contract.ChangeRpm.Payload>());

    private static readonly Hope2Cmd<Cmd, Contract.ChangeRpm.Hope>
        _hope2Cmd =
            hope => Cmd.New(
                hope.AggId.IDFromIdString(),
                hope.Payload,
                EventMeta.New(
                    NameAtt.Get<IEngineAggregateInfo>(),
                    hope.AggId)
            );

    public static EvtCtorT<IEvt, Contract.ChangeRpm.Payload, EventMeta>
        _newEvt =
            (id, payload, meta) => Event.New(
                id,
                TopicAtt.Get<IEvt>(),
                payload.ToBytes(),
                meta.ToBytes()
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
            .AddRootDocCtors()
            .AddBaseBehavior<IEngineAggregateInfo, Schema.Engine, Cmd, IEvt>()
            .AddChangeRpmProjectionFuncs()
            .AddTransient(_ => _guardFunc)
            .AddTransient(_ => _raiseFunc);
    }

    [Topic(Topics.Cmd_v1)]
    public record Cmd(IID AggregateID, Contract.ChangeRpm.Payload Payload, EventMeta Meta)
        : CmdT<Contract.ChangeRpm.Payload, EventMeta>(AggregateID, Payload, Meta)
    {
        public static Cmd New(IID aggregateID, Contract.ChangeRpm.Payload payload, EventMeta meta)
        {
            return new Cmd(aggregateID, payload, meta);
        }
    }

    [Topic(Topics.Evt_v1)]
    public interface IEvt : IEvtT<Contract.ChangeRpm.Payload>
    {
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

    public static class Topics
    {
        public const string Cmd_v1 = "engine:change_rpm:v1";
        public const string Evt_v1 = "engine:rpm_changed:v1";
    }
}