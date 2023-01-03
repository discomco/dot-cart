using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Actors;
using DotCart.Context.Spokes;
using DotCart.Core;
using DotCart.Drivers.Default;
using DotCart.Drivers.NATS;
using DotCart.Drivers.Redis;
using Engine.Behavior;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context;

public static class Initialize
{
    public const string ToRedisDoc_v1 = Contract.Initialize.Topics.Evt_v1 + ":to_redis_doc:v1";
    public const string ToRedisList_v1 = Contract.Initialize.Topics.Evt_v1 + ":to_redis_list:v1";
    public const string SpokeName = "engine:initialize:spoke";

    public static IServiceCollection AddInitializeSpoke(this IServiceCollection services)
    {
        return services
            .AddEngineBehavior()
            .AddInitializeACLFuncs()
            .AddSingletonSequenceBuilder<IEngineAggregateInfo, Schema.Engine>()
            .AddHostedSpokeT<Spoke>()
            .AddSpokedNATSResponder<Spoke, Contract.Initialize.Payload, EventMeta>()
            .AddTransient<IActor<Spoke>, ToRedisDoc>()
            .AddTransient<IActor<Spoke>, ToRedisList>()
            .AddDefaultDrivers<IEngineProjectorInfo, Schema.Engine, Schema.EngineList>();
    }


    [Name(ToRedisDoc_v1)]
    [DbName(DbConstants.DocRedisDbName)]
    public class ToRedisDoc : ProjectionT<
            IRedisStore<Schema.Engine>,
            Schema.Engine,
            Contract.Initialize.Payload,
            EventMeta
        >,
        IActor<Spoke>
    {
        public ToRedisDoc(
            IExchange exchange,
            IRedisStore<Schema.Engine> docStore,
            Evt2Doc<Schema.Engine, Contract.Initialize.Payload, EventMeta> evt2Doc,
            StateCtorT<Schema.Engine> newDoc)
            : base(exchange, docStore, evt2Doc, newDoc)
        {
        }
    }


    public interface ISpoke : ISpokeT<Spoke>
    {
    }

    [Name(SpokeName)]
    public class Spoke : SpokeT<Spoke>, ISpoke
    {
        public Spoke(
            IExchange exchange,
            IProjector projector) : base(exchange, projector)
        {
        }
    }


    [Name(ToRedisList_v1)]
    [DbName(DbConstants.ListRedisDbName)]
    [DocId(IDConstants.EngineListId)]
    public class ToRedisList : ProjectionT<
        IRedisStore<Schema.EngineList>,
        Schema.EngineList,
        Contract.Initialize.Payload,
        EventMeta>, IActor<Spoke>
    {
        public ToRedisList(
            IExchange exchange,
            IRedisStore<Schema.EngineList> docStore,
            Evt2Doc<Schema.EngineList, Contract.Initialize.Payload, EventMeta> evt2Doc,
            StateCtorT<Schema.EngineList> newDoc)
            : base(exchange, docStore, evt2Doc, newDoc)
        {
        }
    }
}