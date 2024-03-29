using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Actors;
using DotCart.Core;
using DotCart.Defaults.EventStore;
using DotCart.Drivers.CouchDB;
using DotCart.Drivers.EventStoreDB;
using DotCart.Drivers.NATS;
using DotCart.Drivers.Redis;
using DotCart.Spokes;
using Engine.Behavior;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context;

public static class ChangeRpm
{
    public const string ToCouchDoc_v1
        = Contract.ChangeRpm.Topics.Evt_v1 + ":to_couch_doc:v1";

    public const string ToRedisDoc_v1
        = Contract.ChangeRpm.Topics.Evt_v1 + ":to_redis_doc:v1";

    public const string ToRedisList_v1
        = Contract.ChangeRpm.Topics.Evt_v1 + ":to_redis_list:v1";

    public const string FromNATS_v1
        = Contract.ChangeRpm.Topics.Hope_v1 + ":from_nats:v1";

    public const string Spoke_v1 = "engine:change_rpm:spoke:v1";

    public static IServiceCollection AddChangeRpmSpoke(this IServiceCollection services)
    {
        return services
            .AddEngineBehavior()
            .AddChangeRpmACLFuncs()
            .AddHostedSpokeT<Spoke>()
            .AddDotCouch<ICouchDocDbInfo, Schema.Engine, Schema.EngineID>()
            .AddTransient<IActorT<Spoke>, ToCouchDoc>()
            .AddDotRedis<IRedisListDbInfo, Schema.EngineList, Schema.EngineListID>()
            .AddDotRedis<IRedisDocDbInfo, Schema.Engine, Schema.EngineID>()
            .AddTransient<IActorT<Spoke>, ToRedisDoc>()
            .AddTransient<IActorT<Spoke>, ToRedisList>()
            .AddProjectorInfra<IEngineProjectorInfo, Schema.Engine, Schema.EngineList>()
            .AddHopeInPipe<IHopePipe, Contract.ChangeRpm.Payload, MetaB>()
            .AddNATSResponderT<Spoke, FromNATS, Contract.ChangeRpm.Payload, MetaB>()
            .AddESDBStore();
    }

    [Name(ToRedisDoc_v1)]
    [DbName(DbConstants.RedisDocDbName)]
    public class ToRedisDoc(
        IExchange exchange,
        IStoreFactoryT<IRedisDocDbInfo, Schema.Engine, Schema.EngineID> storeFactory,
        Evt2Doc<Schema.Engine, Contract.ChangeRpm.Payload, MetaB> evt2Doc,
        StateCtorT<Schema.Engine> newDoc)
        : ProjectionT<
            IRedisDocDbInfo,
            Schema.Engine,
            Contract.ChangeRpm.Payload,
            MetaB,
            Schema.EngineID>(exchange,
            storeFactory,
            evt2Doc,
            newDoc), IActorT<Spoke>;


    [Name(Spoke_v1)]
    public class Spoke(
        IExchange exchange,
        IProjector projector)
        : SpokeT<Spoke>(exchange, projector);

    [DocId(IDConstants.EngineListId)]
    [DbName(DbConstants.RedisListDbName)]
    [Name(ToRedisList_v1)]
    public class ToRedisList(
        IExchange exchange,
        IStoreFactoryT<IRedisListDbInfo, Schema.EngineList, Schema.EngineListID> storeFactory,
        Evt2Doc<Schema.EngineList, Contract.ChangeRpm.Payload, MetaB> evt2Doc,
        StateCtorT<Schema.EngineList> newDoc)
        : ProjectionT<
            IRedisListDbInfo,
            Schema.EngineList,
            Contract.ChangeRpm.Payload, MetaB, Schema.EngineListID>(exchange,
            storeFactory,
            evt2Doc,
            newDoc), IActorT<Spoke>;

    [Name(FromNATS_v1)]
    public class FromNATS(
        INATSResponderDriverT<Contract.ChangeRpm.Payload> driver,
        IExchange exchange,
        IPipeBuilderT<IHopePipe, Contract.ChangeRpm.Payload> builder)
        : ResponderT<
            Spoke,
            Contract.ChangeRpm.Payload,
            IHopePipe>(driver, exchange, builder);

    public interface IHopePipe
        : IPipeInfoB;


    [Name(ToCouchDoc_v1)]
    [DbName(DbConstants.CouchDocDbName)]
    public class ToCouchDoc(
        IExchange exchange,
        IStoreFactoryT<ICouchDocDbInfo, Schema.Engine, Schema.EngineID> storeFactory,
        Evt2Doc<Schema.Engine, Contract.ChangeRpm.Payload, MetaB> evt2Doc,
        StateCtorT<Schema.Engine> newDoc)
        : ProjectionT<ICouchDocDbInfo,
            Schema.Engine,
            Contract.ChangeRpm.Payload,
            MetaB,
            Schema.EngineID>(exchange,
            storeFactory,
            evt2Doc,
            newDoc), IActorT<Spoke>;
}