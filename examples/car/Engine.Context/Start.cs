using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Actors;
using DotCart.Context.Spokes;
using DotCart.Core;
using DotCart.Drivers.Default;
using DotCart.Drivers.NATS;
using DotCart.Drivers.RabbitMQ;
using DotCart.Drivers.Redis;
using Engine.Behavior;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context;

public static class Start
{
    public const string ToRedisDoc_v1 = Contract.Start.Topics.Evt_v1 + ":to_redis_doc:v1";
    public const string ToRedisList_v1 = Contract.Start.Topics.Evt_v1 + ":to_redis_list:v1";
    public const string ToRabbitMq_v1 = Contract.Start.Topics.Fact_v1 + ":to_rabbit_mq:v1";
    public const string FromNATS_v1 = Contract.Start.Topics.Hope_v1 + ":from_nats:v1";

    public const string Spoke_v1 = "engine:start:spoke:v1";

    public static IServiceCollection AddStartSpoke(this IServiceCollection services)
    {
        return services
            .AddEngineBehavior()
            .AddStartACLFuncs()
            .AddStartProjectionFuncs()
            .AddSingletonSequenceBuilder<IEngineAggregateInfo, Schema.Engine>()
            .AddTransient<IActorT<Spoke>, ToRedisDoc>()
            .AddTransient<IActorT<Spoke>, ToRedisList>()
            .AddDefaultDrivers<IEngineProjectorInfo, Schema.Engine, Schema.EngineList>()
            .AddHostedSpokeT<Spoke>()
            .AddNATSResponder<Spoke, FromNATS, Contract.Start.Payload, EventMeta>()
            .AddRabbitMQEmitter<Spoke, ToRabbitMq, Contract.Start.Payload, EventMeta>();
    }

    public interface IToRedisDoc : IActorT<Spoke>
    {
    }

    [Name(ToRedisDoc_v1)]
    [DbName(DbConstants.DocRedisDbName)]
    public class ToRedisDoc : ProjectionT<
        IRedisStore<Schema.Engine>,
        Schema.Engine,
        Contract.Start.Payload, EventMeta>, IToRedisDoc
    {
        public ToRedisDoc(IExchange exchange,
            IRedisStore<Schema.Engine> docStore,
            Evt2Doc<Schema.Engine, Contract.Start.Payload, EventMeta> evt2Doc,
            StateCtorT<Schema.Engine> newDoc)
            : base(exchange, docStore, evt2Doc, newDoc)
        {
        }
    }

    [Name(Spoke_v1)]
    public class Spoke : SpokeT<Spoke>
    {
        public Spoke(
            IExchange exchange,
            IProjector projector) : base(exchange, projector)
        {
        }
    }

    [DbName(DbConstants.ListRedisDbName)]
    [DocId(IDConstants.EngineListId)]
    [Name(ToRedisList_v1)]
    public class ToRedisList
        : ProjectionT<
            IRedisStore<Schema.EngineList>,
            Schema.EngineList,
            Contract.Start.Payload,
            EventMeta>, IActorT<Spoke>
    {
        public ToRedisList(IExchange exchange,
            IRedisStore<Schema.EngineList> docStore,
            Evt2Doc<Schema.EngineList, Contract.Start.Payload, EventMeta> evt2Doc,
            StateCtorT<Schema.EngineList> newDoc)
            : base(exchange, docStore, evt2Doc, newDoc)
        {
        }
    }

    [Name(ToRabbitMq_v1)]
    public class ToRabbitMq
        : EmitterT<Spoke, Contract.Start.Payload, EventMeta>
    {
        public ToRabbitMq(
            IRmqEmitterDriverT<Contract.Start.Payload, EventMeta> driver,
            IExchange exchange,
            Evt2Fact<Contract.Start.Payload, EventMeta> evt2Fact) : base(driver, exchange, evt2Fact)
        {
        }
    }

    [Name(FromNATS_v1)]
    public class FromNATS
        : ResponderT<
            Spoke,
            Contract.Start.Payload,
            EventMeta>
    {
        public FromNATS(
            INATSResponderDriverT<Contract.Start.Payload> driver,
            IExchange exchange,
            ISequenceBuilder builder,
            Hope2Cmd<Contract.Start.Payload, EventMeta> hope2Cmd)
            : base(driver, exchange, builder, hope2Cmd)
        {
        }
    }
}