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

public static partial class Inject
{
}

public static class Initialize
{
    public static readonly EvtCtorT<
            Contract.Initialize.Payload,
            EventMeta>
        _newEvt = Event.New<Contract.Initialize.Payload>;

    private static readonly Evt2Fact<Contract.Initialize.Payload, EventMeta>
        _evt2Fact =
            evt => FactT<Contract.Initialize.Payload, EventMeta>.New(
                evt.AggregateId,
                evt.Data.FromBytes<Contract.Initialize.Payload>(),
                evt.MetaData.FromBytes<EventMeta>()
            );

    private static readonly Fact2Msg<byte[], Contract.Initialize.Payload, EventMeta>
        _fact2Msg =
            fact => fact.ToBytes();

    private static readonly Hope2Cmd<Contract.Initialize.Payload, EventMeta>
        _hope2Cmd =
            hope =>
            {
                var id = Schema.EngineID.New();
                return Command.New<Contract.Initialize.Payload>(
                    id
                    , hope.Payload.ToBytes()
                    , EventMeta.New(
                        NameAtt.Get<IEngineAggregateInfo>(),
                        id.Id()).ToBytes()
                );
            };

    private static readonly Evt2Doc<Schema.Engine, Contract.Initialize.Payload, EventMeta>
        _evt2Doc =
            (state, evt) =>
            {
                if (evt == null) return state;
                if (evt.GetPayload<Contract.Initialize.Payload>() == null) return state;
                var newState = state with { };
                newState.Id = evt.AggregateId;
                newState.Details = evt.GetPayload<Contract.Initialize.Payload>().Details;
                newState.Status = Schema.EngineStatus.Initialized;
                return newState;
            };

    private static readonly Evt2DocValidator<Schema.Engine, Contract.Initialize.Payload, EventMeta>
        _evt2DocVal =
            (_, output, evt) => output.Id == evt.AggregateId
                                && output.Status.HasFlagFast(Schema.EngineStatus.Initialized);

    private static readonly Evt2Doc<Schema.EngineList, Contract.Initialize.Payload, EventMeta>
        _evt2List =
            (list, evt) =>
            {
                if (list.Items.Any(x => x.Key == evt.AggregateId))
                {
                    list.Items[evt.AggregateId].Status = Schema.EngineStatus.Initialized;
                    return list;
                }

                var newList = list with
                {
                    Items = ImmutableDictionary<string, Schema.EngineListItem>.Empty.AddRange(list.Items)
                };
                newList.Items = list.Items.Add(evt.AggregateId, Schema.EngineListItem.New(
                    evt.AggregateId,
                    evt.GetPayload<Contract.Initialize.Payload>().Details.Name,
                    Schema.EngineStatus.Initialized,
                    0));
                return newList;
            };

    private static readonly Evt2DocValidator<Schema.EngineList, Contract.Initialize.Payload, EventMeta>
        _evt2ListVal =
            (input, output, evt) =>
            {
                var result =
                    !input.Items.Any()
                    || input.Items.All(item => item.Key != evt.AggregateId);
                result = result
                         && output.Items.Any(item => item.Key == evt.AggregateId)
                         && output.Items[evt.AggregateId].Status.HasFlagFast(Schema.EngineStatus.Initialized);
                return result;
            };

    private static readonly GuardFuncT<Schema.Engine, Contract.Initialize.Payload, EventMeta>
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

    private static readonly RaiseFuncT<Schema.Engine, Contract.Initialize.Payload, EventMeta>
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
            .AddTransient(_ => _fact2Msg);
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
            .AddBaseBehavior<IEngineAggregateInfo, Schema.Engine, Contract.Initialize.Payload, EventMeta>()
            .AddTransient(_ => _guardFunc)
            .AddTransient(_ => _raiseFunc)
            .AddTransient(_ => _newEvt);
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