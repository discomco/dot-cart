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

public static class Start
{
    public const string OnInitialized_v1 = "engine:on_initialized:start:v1";


    private static readonly Evt2Fact<Contract.Start.Payload, EventMeta>
        _evt2Fact =
            evt =>
                FactT<Contract.Start.Payload, EventMeta>.New(
                    evt.AggregateId,
                    evt.GetPayload<Contract.Start.Payload>(),
                    evt.GetMeta<EventMeta>()
                );


    private static readonly Fact2Msg<byte[], Contract.Start.Payload, EventMeta>
        _fact2Msg =
            fact => fact.ToBytes(); 

    private static readonly Hope2Cmd<Contract.Start.Payload, EventMeta>
        _hope2Cmd =
            hope =>
                Command.New<Contract.Start.Payload>(
                    hope.AggId.IDFromIdString(),
                    hope.Payload.ToBytes(),
                    EventMeta.New(NameAtt.Get<IEngineAggregateInfo>(), hope.AggId).ToBytes()
                );


    private static readonly Evt2Doc<Schema.Engine, Contract.Start.Payload, EventMeta>
        _evt2Doc =
            (doc, _) =>
            {
                var newDoc = doc with { };
                newDoc.Status = newDoc.Status
                    .UnsetFlag(Schema.EngineStatus.Stopped)
                    .SetFlag(Schema.EngineStatus.Started);
                return newDoc;
            };

    private static readonly Evt2DocValidator<Schema.Engine, Contract.Start.Payload, EventMeta>
        _evt2DocVal =
            (input, output, _)
                => !input.Status.HasFlagFast(Schema.EngineStatus.Started)
                   && output.Status.HasFlagFast(Schema.EngineStatus.Started)
                   && !output.Status.HasFlagFast(Schema.EngineStatus.Stopped);

    private static readonly Evt2Doc<Schema.EngineList, Contract.Start.Payload, EventMeta>
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

    private static readonly Evt2DocValidator<Schema.EngineList, Contract.Start.Payload, EventMeta>
        _evt2ListVal =
            (input, output, evt) =>
            {
                return output.Items.Any(it => it.Key == evt.AggregateId)
                       && output.Items[evt.AggregateId].Status.HasFlagFast(Schema.EngineStatus.Started)
                       && !output.Items[evt.AggregateId].Status.HasFlagFast(Schema.EngineStatus.Stopped);
            };


    private static readonly GuardFuncT<Schema.Engine, Contract.Start.Payload, EventMeta>
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

    private static readonly RaiseFuncT<Schema.Engine, Contract.Start.Payload, EventMeta>
        _raiseFunc =
            (cmd, _) =>
            {
                return new[]
                {
                    _newEvt(cmd.AggregateID, cmd.Data, cmd.MetaData)
                };
            };


    public static readonly EvtCtorT<Contract.Start.Payload, EventMeta>
        _newEvt =
            Event.New<Contract.Start.Payload>;

    private static readonly Evt2Cmd<Contract.Start.Payload, Contract.Initialize.Payload, EventMeta>
        _shouldStartOnInitialized =
            (evt, _) =>
                Command.New<Contract.Start.Payload>(
                    evt.AggregateId.IDFromIdString(),
                    Contract.Start.Payload.New.ToBytes(),
                    EventMeta.New(NameAtt.Get<IEngineAggregateInfo>(), evt.AggregateId).ToBytes());

    public static IServiceCollection AddStartACLFuncs(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2Fact)
            .AddTransient(_ => _evt2Doc)
            .AddTransient(_ => _hope2Cmd)
            .AddTransient(_ => _fact2Msg);
    }

    public static IServiceCollection AddStartBehavior(this IServiceCollection services)
    {
        return services
            .AddMetaCtor()
            .AddRootDocCtors()
            .AddBaseBehavior<IEngineAggregateInfo, Schema.Engine, Contract.Start.Payload, EventMeta>()
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
}