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

public static class ChangeDetails
{
    public const string Spoke_v1
        = "engine:change_details:spoke:v1";
    public const string ToCouchDoc_v1
        = Contract.ChangeDetails.Topics.Evt_v1 + ":to_couch_doc:v1";
    public const string ToRedisDoc_v1
        = Contract.ChangeDetails.Topics.Evt_v1 + ":to_redis_doc:v1";
    public const string ToRedisList_v1
        = Contract.ChangeDetails.Topics.Evt_v1 + ":to_redis_list:v1";
    public const string ToRabbitMq_v1
        = Contract.ChangeDetails.Topics.Fact_v1 + ":to_rabbit_mq:v1";
    public const string FromRabbitMqRetro_v1
        = Contract.ChangeDetails.Topics.Fact_v1 + ":from_rabbit_mq_retro:v1";

    public const string FromNATS_v1 = Contract.ChangeDetails.Topics.Hope_v1 + ":from_nats:v1";


    public static IServiceCollection AddChangeDetailsSpoke(this IServiceCollection services)
    {
        return services
            .AddEngineBehavior()
            .AddChangeDetailsACLFuncs()
            .AddHostedSpokeT<Spoke>()
            .AddESDBStore()
            .AddProjectorInfra<IEngineProjectorInfo, Schema.Engine, Schema.EngineList>()
            .AddDotCouch<ICouchDocDbInfo, Schema.Engine, Schema.EngineID>()
            .AddTransient<IActorT<Spoke>, ToCouchDoc>()
            .AddDotRedis<IRedisListDbInfo, Schema.EngineList, Schema.EngineListID>()
            .AddDotRedis<IRedisDocDbInfo, Schema.Engine, Schema.EngineID>()
            .AddTransient<IActorT<Spoke>, ToRedisDoc>()
            .AddTransient<IActorT<Spoke>, ToRedisList>()
            .AddHopeInPipe<IHopePipe, Contract.ChangeDetails.Payload, MetaB>()
            .AddNATSResponderT<Spoke,
                FromNATS,
                Contract.ChangeDetails.Payload,
                MetaB>()
            .AddRabbitMqEmitter<Spoke,
                ToRabbitMq,
                Contract.ChangeDetails.Payload,
                MetaB>()
            .AddRabbitMqListener<Spoke,
                FromRabbitMqRetro,
                Dummy,
                MetaB,
                Contract.ChangeDetails.Payload,
                IRetroPipe>();
    }


    [Name(Spoke_v1)]
    public class Spoke(
        IExchange exchange,
        IProjector projector)
        : SpokeT<Spoke>(exchange, projector);

    [Name(ToRedisDoc_v1)]
    [DbName(DbConstants.RedisDocDbName)]
    public class ToRedisDoc(
        IExchange exchange,
        IStoreFactoryT<IRedisDocDbInfo, Schema.Engine, Schema.EngineID> storeFactory,
        Evt2Doc<Schema.Engine, Contract.ChangeDetails.Payload, MetaB> evt2Doc,
        StateCtorT<Schema.Engine> newDoc)
        : ProjectionT<IRedisDocDbInfo,
            Schema.Engine,
            Contract.ChangeDetails.Payload, MetaB, Schema.EngineID>(exchange,
            storeFactory,
            evt2Doc,
            newDoc), IActorT<Spoke>;

    [DbName(DbConstants.RedisListDbName)]
    [Name(ToRedisList_v1)]
    [DocId(IDConstants.EngineListId)]
    public class ToRedisList(
        IExchange exchange,
        IStoreFactoryT<IRedisListDbInfo, Schema.EngineList, Schema.EngineListID> storeFactory,
        Evt2Doc<Schema.EngineList, Contract.ChangeDetails.Payload, MetaB> evt2Doc,
        StateCtorT<Schema.EngineList> newDoc)
        : ProjectionT<
            IRedisListDbInfo,
            Schema.EngineList,
            Contract.ChangeDetails.Payload,
            MetaB, Schema.EngineListID>(exchange,
            storeFactory,
            evt2Doc,
            newDoc), IActorT<Spoke>;

    [Name(ToRabbitMq_v1)]
    public class ToRabbitMq(
        IRmqEmitterDriverT<Contract.ChangeDetails.Payload, MetaB> driver,
        IExchange exchange,
        Evt2Fact<Contract.ChangeDetails.Payload, MetaB> evt2Fact)
        : EmitterT<
            Spoke,
            Contract.ChangeDetails.Payload,
            MetaB>(driver,
            exchange,
            evt2Fact);


    [Name(FromNATS_v1)]
    public class FromNATS(
        INATSResponderDriverT<Contract.ChangeDetails.Payload> driver,
        IExchange exchange,
        IPipeBuilderT<IHopePipe, Contract.ChangeDetails.Payload> builder)
        : ResponderT<
            Spoke,
            Contract.ChangeDetails.Payload,
            IHopePipe>(driver, exchange, builder);

    [Name(FromRabbitMqRetro_v1)]
    public class FromRabbitMqRetro(
        IRmqListenerDriverT<Contract.ChangeDetails.Payload> driver,
        IExchange exchange,
        IPipeBuilderT<IRetroPipe, Contract.ChangeDetails.Payload> pipeBuilder)
        : ListenerT<
            Spoke,
            Dummy,
            MetaB,
            Contract.ChangeDetails.Payload,
            IRetroPipe>(driver, exchange, pipeBuilder);

    public interface IHopePipe
        : IPipeInfoB;

    public interface IRetroPipe
        : IPipeInfoB;


    [Name(ToCouchDoc_v1)]
    [DbName(DbConstants.CouchDocDbName)]
    public class ToCouchDoc(
        IExchange exchange,
        IStoreFactoryT<ICouchDocDbInfo, Schema.Engine, Schema.EngineID> storeFactory,
        Evt2Doc<Schema.Engine, Contract.ChangeDetails.Payload, MetaB> evt2Doc,
        StateCtorT<Schema.Engine> newDoc)
        : ProjectionT<
            ICouchDocDbInfo,
            Schema.Engine,
            Contract.ChangeDetails.Payload,
            MetaB,
            Schema.EngineID>(exchange, storeFactory, evt2Doc, newDoc), IActorT<Spoke>;
}