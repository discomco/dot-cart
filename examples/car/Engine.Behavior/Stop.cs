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


    private static readonly Evt2Fact<Contract.Stop.Fact, IEvt>
        _evt2Fact =
            evt => Contract.Stop.Fact.New(
                evt.AggregateId,
                evt.GetPayload<Contract.Stop.Payload>()
            );

    private static readonly Evt2Doc<Schema.Engine, IEvt>
        _evt2Doc =
            (doc, _) =>
            {
                var newDoc = doc with { };
                newDoc.Status = newDoc.Status
                    .UnsetFlag(Schema.EngineStatus.Started)
                    .SetFlag(Schema.EngineStatus.Stopped);
                return newDoc;
            };

    private static readonly Evt2DocValidator<Schema.Engine, IEvt>
        _evt2DocVal =
            (_, output, _)
                => output.Status.HasFlagFast(Schema.EngineStatus.Stopped)
                   && !output.Status.HasFlagFast(Schema.EngineStatus.Started);


    private static readonly Evt2Doc<Schema.EngineList, IEvt>
        _evt2List =
            (lst, evt) =>
            {
                if (lst.Items.All(it => it.Key != evt.AggregateId))
                    return lst;
                var newList = lst with
                {
                    Items = ImmutableDictionary.CreateRange(lst.Items)
                };
                newList.Items[evt.AggregateId].Status
                    = newList.Items[evt.AggregateId].Status.UnsetFlag(Schema.EngineStatus.Started);
                newList.Items[evt.AggregateId].Status
                    = newList.Items[evt.AggregateId].Status.SetFlag(Schema.EngineStatus.Stopped);
                return newList;
            };

    private static readonly Hope2Cmd<Cmd, Contract.Stop.Hope>
        _hope2Cmd =
            hope =>
                Cmd.New(hope.AggId.IDFromIdString(), hope.Payload);

    private static readonly GuardFuncT<Schema.Engine, Cmd>
        _guardFunc =
            (cmd, state) =>
            {
                var fbk = Feedback.New(cmd.AggregateID.Id());
                try
                {
                    Guard.Against.EngineNotStarted(state);
                }
                catch (Exception e)
                {
                    Log.Debug(e.InnerAndOuter());
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

    public static readonly EvtCtorT<IEvt, Contract.Stop.Payload, EventMeta>
        _newEvt =
            (id, payload, meta) => Event.New(id,
                TopicAtt.Get<IEvt>(),
                payload.ToBytes(),
                meta.ToBytes());

    private static readonly Evt2Cmd<Cmd, ChangeRpm.IEvt>
        _shouldStopOnZeroPower =
            (evt, state) =>
            {
                var eng = (Schema.Engine)state;
                var pld = evt.GetPayload<Contract.ChangeRpm.Payload>();
                return eng.Power + pld.Delta <= 0
                    ? Cmd.New(evt.AggregateID, Contract.Stop.Payload.New())
                    : null;
            };

    private static readonly Evt2DocValidator<Schema.EngineList, IEvt>
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
            .AddRootDocCtors()
            .AddBaseBehavior<IEngineAggregateInfo, Schema.Engine, Cmd, IEvt>()
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


    [Topic(Topics.Cmd_v1)]
    public record Cmd(IID AggregateID, Contract.Stop.Payload Payload, EventMeta Meta)
        : CmdT<Contract.Stop.Payload, EventMeta>(AggregateID, Payload, Meta)
    {
        public static Cmd New(IID ID, Contract.Stop.Payload payload)
        {
            return new Cmd(ID, payload, EventMeta.New(NameAtt.Get<IEngineAggregateInfo>(), ID.Id()));
        }
    }


    [Topic(Topics.Evt_v1)]
    public interface IEvt : IEvtT<Contract.Stop.Payload>
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
    }

    public static class Topics
    {
        public const string Evt_v1 = "engine:stopped:v1";
        public const string Cmd_v1 = "engine:stop:v1";
    }

    // [Name(OnZeroPower_v1)]
    // public class OnZeroPowerStop : AggregatePolicyT<ChangeRpm.IEvt, Cmd>
    // {
    //     public OnZeroPowerStop(
    //         IExchange exchange,
    //         Evt2Cmd<Cmd, ChangeRpm.IEvt> evt2Cmd)
    //         : base(exchange, evt2Cmd)
    //     {
    //     }
    // }
}