using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Actors;
using DotCart.Core;
using DotCart.Defaults;
using DotCart.Defaults.EventStore;
using DotCart.Defaults.RabbitMq;
using DotCart.Drivers.CouchDB;
using DotCart.Drivers.EventStoreDB;
using DotCart.Drivers.NATS;
using DotCart.Drivers.RabbitMQ;
using DotCart.Drivers.Redis;
using DotCart.Spokes;
using Engine.Behavior;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context;

public static class Initialize
{
    public const string ToRedisDoc_v1 = Contract.Initialize.Topics.Evt_v1 + ":to_redis_doc:v1";
    public const string ToCouchDoc_v1 = Contract.Initialize.Topics.Evt_v1 + ":to_couch_doc:v1";
    public const string ToRedisList_v1 = Contract.Initialize.Topics.Evt_v1 + ":to_redis_list:v1";
    public const string ToRabbitMq_v1 = Contract.Initialize.Topics.Fact_v1 + ":to_rabbit_mq:v1";
    public const string FromRabbitMqRetro_v1 = Contract.Initialize.Topics.Fact_v1 + ":from_rabbit_mq_retro:v1";

    public const string FromNATS_v1 = Contract.Initialize.Topics.Hope_v1 + ":from_nats:v1";

    public const string SpokeName = "engine:initialize:spoke";

    public static IServiceCollection AddInitializeSpoke(this IServiceCollection services)
    {
        return services
            .AddEngineBehavior()
            .AddInitializeACLFuncs()
            .AddHopeInPipe<IHopePipe, Contract.Initialize.Payload, MetaB>()
            .AddESDBStore()
            .AddNATSResponderT<Spoke, FromNATS, Contract.Initialize.Payload, MetaB>()
            .AddProjectorInfra<IEngineProjectorInfo, Schema.Engine, Schema.EngineList>()
            .AddDotCouch<ICouchDocDbInfo, Schema.Engine, Schema.EngineID>()
            .AddTransient<IActorT<Spoke>, ToCouchDoc>()
            .AddTransient<IActorT<Spoke>, ToRedisDoc>()
            .AddTransient<IActorT<Spoke>, ToRedisList>()
            .AddDotRedis<IRedisListDbInfo, Schema.EngineList, Schema.EngineListID>()
            .AddDotRedis<IRedisDocDbInfo, Schema.Engine, Schema.EngineID>()
            .AddRabbitMqEmitter<Spoke, ToRabbitMq, Contract.Initialize.Payload, MetaB>()
            .AddRabbitMqListener<Spoke,
                FromRabbitMqRetro,
                Dummy,
                MetaB,
                Contract.Initialize.Payload,
                IRetroPipe>()
            .AddHostedSpokeT<Spoke>();
    }


    [Name(ToRedisDoc_v1)]
    [DbName(DbConstants.RedisDocDbName)]
    public class ToRedisDoc(
        IExchange exchange,
        IStoreFactoryT<IRedisDocDbInfo, Schema.Engine, Schema.EngineID> storeFactory,
        Evt2Doc<Schema.Engine, Contract.Initialize.Payload, MetaB> evt2Doc,
        StateCtorT<Schema.Engine> newDoc)
        : ProjectionT<
                IRedisDocDbInfo,
                Schema.Engine,
                Contract.Initialize.Payload,
                MetaB,
                Schema.EngineID
            >(exchange,
                storeFactory,
                evt2Doc,
                newDoc),
            IActorT<Spoke>;


    public interface ISpoke
        : ISpokeT<Spoke>;

    [Name(SpokeName)]
    public class Spoke(
        IExchange exchange,
        IProjector projector)
        : SpokeT<Spoke>(exchange, projector), ISpoke;


    [Name(ToRedisList_v1)]
    [DbName(DbConstants.RedisListDbName)]
    [DocId(IDConstants.EngineListId)]
    public class ToRedisList(
        IExchange exchange,
        IStoreFactoryT<IRedisListDbInfo, Schema.EngineList, Schema.EngineListID> storeFactory,
        Evt2Doc<Schema.EngineList, Contract.Initialize.Payload, MetaB> evt2Doc,
        StateCtorT<Schema.EngineList> newDoc)
        : ProjectionT<
            IRedisListDbInfo,
            Schema.EngineList,
            Contract.Initialize.Payload,
            MetaB, Schema.EngineListID>(exchange,
            storeFactory,
            evt2Doc,
            newDoc), IActorT<Spoke>;

    [Name(ToRabbitMq_v1)]
    public class ToRabbitMq(
        IRmqEmitterDriverT<Contract.Initialize.Payload, MetaB> driver,
        IExchange exchange,
        Evt2Fact<Contract.Initialize.Payload, MetaB> evt2Fact)
        : EmitterT<Spoke, Contract.Initialize.Payload, MetaB>(driver,
            exchange,
            evt2Fact);

    [Name(FromNATS_v1)]
    public class FromNATS(
        INATSResponderDriverT<Contract.Initialize.Payload> driver,
        IExchange exchange,
        IPipeBuilderT<IHopePipe, Contract.Initialize.Payload> builder)
        : ResponderT<
            Spoke,
            Contract.Initialize.Payload,
            IHopePipe>(driver, exchange, builder);

    public interface IHopePipe
        : IPipeInfoB;

    public interface IRetroPipe
        : IPipeInfoB;

    [Name(FromRabbitMqRetro_v1)]
    public class FromRabbitMqRetro(
        IRmqListenerDriverT<Contract.Initialize.Payload> driver,
        IExchange exchange,
        IPipeBuilderT<IRetroPipe, Contract.Initialize.Payload> pipeBuilder)
        : ListenerT<Spoke, Dummy, MetaB, Contract.Initialize.Payload, IRetroPipe>(driver, exchange, pipeBuilder);

    [Name(ToCouchDoc_v1)]
    [DbName(DbConstants.CouchDocDbName)]
    public class ToCouchDoc(
        IExchange exchange,
        IStoreFactoryT<ICouchDocDbInfo, Schema.Engine, Schema.EngineID> storeFactory,
        Evt2Doc<Schema.Engine, Contract.Initialize.Payload, MetaB> evt2Doc,
        StateCtorT<Schema.Engine> newDoc)
        : ProjectionT<ICouchDocDbInfo,
            Schema.Engine,
            Contract.Initialize.Payload,
            MetaB,
            Schema.EngineID>(exchange,
            storeFactory,
            evt2Doc,
            newDoc), IActorT<ISpoke>;
}