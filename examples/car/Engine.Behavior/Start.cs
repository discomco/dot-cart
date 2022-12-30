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

public static class Start
{
    public const string OnInitialized_v1 = "engine:on_initialized:start:v1";


    private static readonly Evt2Fact<FactT<Contract.Start.Payload>, IEvt>
        _evt2Fact =
            evt =>
                FactT<Contract.Start.Payload>.New(
                    evt.AggregateId,
                    evt.GetPayload<Contract.Start.Payload>()
                );

    private static readonly Hope2Cmd<Cmd, Contract.Start.Hope>
        _hope2Cmd =
            hope =>
                Cmd.New(hope.AggId.IDFromIdString(), hope.Payload);

    private static readonly Evt2Doc<Schema.Engine, IEvt>
        _evt2Doc =
            (doc, _) =>
            {
                var newDoc = doc with { };
                newDoc.Status = newDoc.Status
                    .UnsetFlag(Schema.EngineStatus.Stopped)
                    .SetFlag(Schema.EngineStatus.Started);
                return newDoc;
            };

    private static readonly Evt2DocValidator<Schema.Engine, IEvt>
        _evt2DocVal =
            (input, output, _)
                => !input.Status.HasFlagFast(Schema.EngineStatus.Started)
                   && output.Status.HasFlagFast(Schema.EngineStatus.Started)
                   && !output.Status.HasFlagFast(Schema.EngineStatus.Stopped);

    private static readonly Evt2Doc<Schema.EngineList, IEvt>
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
                    = newLst.Items[evt.AggregateId].Status.UnsetFlag(Schema.EngineStatus.Stopped);
                newLst.Items[evt.AggregateId].Status
                    = newLst.Items[evt.AggregateId].Status.SetFlag(Schema.EngineStatus.Started);
                return newLst;
            };

    private static readonly Evt2DocValidator<Schema.EngineList, IEvt>
        _evt2ListVal =
            (input, output, evt) =>
            {
                return output.Items.Any(it => it.Key == evt.AggregateId)
                       && output.Items[evt.AggregateId].Status.HasFlagFast(Schema.EngineStatus.Started)
                       && !output.Items[evt.AggregateId].Status.HasFlagFast(Schema.EngineStatus.Stopped);
            };


    private static readonly GuardFuncT<Schema.Engine, Cmd>
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

    private static readonly RaiseFuncT<Schema.Engine, Cmd>
        _raiseFunc =
            (cmd, _) =>
            {
                return new[]
                {
                    _newEvt(cmd.AggregateID, cmd.Payload, cmd.Meta)
                };
            };


    public static readonly EvtCtorT<IEvt, Contract.Start.Payload, EventMeta>
        _newEvt =
            (id, payload, meta) => Event.New(id,
                TopicAtt.Get<IEvt>(),
                payload.ToBytes(),
                meta.ToBytes());

    private static readonly Evt2Cmd<Cmd, Initialize.IEvt>
        _shouldStartOnInitialized =
            (evt, _) =>
                Cmd.New(evt.AggregateId.IDFromIdString(), Contract.Start.Payload.New);

    public static IServiceCollection AddStartACLFuncs(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2Fact)
            .AddTransient(_ => _evt2Doc)
            .AddTransient(_ => _hope2Cmd);
    }

    public static IServiceCollection AddStartBehavior(this IServiceCollection services)
    {
        return services
            .AddRootDocCtors()
            .AddBaseBehavior<IEngineAggregateInfo, Schema.Engine, Cmd, IEvt>()
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


    public static class Topics
    {
        public const string Cmd_v1 = "engine:start:v1";
        public const string Evt_v1 = "engine:started:v1";
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

        public static Exception New(string message)
        {
            return new Exception(message);
        }
    }

    [Topic(Topics.Cmd_v1)]
    public record Cmd(IID AggregateID, Contract.Start.Payload Payload, EventMeta Meta)
        : CmdT<Contract.Start.Payload, EventMeta>(AggregateID, Payload, Meta)
    {
        public static Cmd New(IID aggregateID, Contract.Start.Payload payload)
        {
            return new Cmd(
                aggregateID,
                payload,
                EventMeta.New(
                    NameAtt.Get<IEngineAggregateInfo>(),
                    aggregateID.Id()));
        }
    }


    [Topic(Topics.Evt_v1)]
    public interface IEvt : IEvtT<Contract.Start.Payload>
    {
    }
}