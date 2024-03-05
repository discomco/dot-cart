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
using DotCart.Drivers.EventStoreDB;
using DotCart.Drivers.NATS;
using DotCart.Drivers.RabbitMQ;
using DotCart.Drivers.Redis;
using DotCart.Spokes;
using Engine.Behavior;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context;

public static class Start
{
    public const string ToRedisDoc_v1 = Contract.Start.Topics.Evt_v1 + ":to_redis_doc:v1";
    public const string ToRedisList_v1 = Contract.Start.Topics.Evt_v1 + ":to_redis_list:v1";
    public const string ToRabbitMq_v1 = Contract.Start.Topics.Fact_v1 + ":to_rabbit_mq:v1";
    public const string FromRabbitMqRetro_v1 = Contract.Start.Topics.Fact_v1 + ":from_rabbit_mq_retro:v1";

    public const string FromNATS_v1 = Contract.Start.Topics.Hope_v1 + ":from_nats:v1";

    public const string Spoke_v1 = "engine:start:spoke:v1";

    public static IServiceCollection AddStartSpoke(this IServiceCollection services)
    {
        return services
            .AddEngineBehavior()
            .AddStartACLFuncs()
            .AddStartProjectionFuncs()
            .AddHopeInPipe<IHopePipe, Contract.Start.Payload, MetaB>()
            .AddProjectorInfra<IEngineProjectorInfo, Schema.Engine, Schema.EngineList>()
            .AddHostedSpokeT<Spoke>()
            .AddDotRedis<IRedisListDbInfo, Schema.EngineList, Schema.EngineListID>()
            .AddDotRedis<IRedisDocDbInfo, Schema.Engine, Schema.EngineID>()
            .AddTransient<IActorT<Spoke>, ToRedisDoc>()
            .AddTransient<IActorT<Spoke>, ToRedisList>()
            .AddNATSResponderT<Spoke, FromNATS, Contract.Start.Payload, MetaB>()
            .AddRabbitMqEmitter<Spoke, ToRabbitMq, Contract.Start.Payload, MetaB>()
            .AddRabbitMqListener<Spoke,
                FromRabbitMqRetro,
                Dummy,
                MetaB,
                Contract.Start.Payload,
                IRetroPipe>()
            .AddESDBStore();
    }

    public interface IToRedisDoc : IActorT<Spoke>;

    [Name(ToRedisDoc_v1)]
    [DbName(DbConstants.RedisDocDbName)]
    public class ToRedisDoc(
        IExchange exchange,
        IStoreFactoryT<IRedisDocDbInfo, Schema.Engine, Schema.EngineID> storeFactory,
        Evt2Doc<Schema.Engine, Contract.Start.Payload, MetaB> evt2Doc,
        StateCtorT<Schema.Engine> newDoc)
        : ProjectionT<
            IRedisDocDbInfo,
            Schema.Engine,
            Contract.Start.Payload, MetaB, Schema.EngineID>(exchange,
            storeFactory,
            evt2Doc,
            newDoc), IToRedisDoc;

    [Name(Spoke_v1)]
    public class Spoke(
        IExchange exchange,
        IProjector projector) : SpokeT<Spoke>(exchange, projector);

    [DbName(DbConstants.RedisListDbName)]
    [DocId(IDConstants.EngineListId)]
    [Name(ToRedisList_v1)]
    public class ToRedisList(
        IExchange exchange,
        IStoreFactoryT<IRedisListDbInfo, Schema.EngineList, Schema.EngineListID> storeFactory,
        Evt2Doc<Schema.EngineList, Contract.Start.Payload, MetaB> evt2Doc,
        StateCtorT<Schema.EngineList> newDoc)
        : ProjectionT<
            IRedisListDbInfo,
            Schema.EngineList,
            Contract.Start.Payload,
            MetaB, Schema.EngineListID>(exchange,
            storeFactory,
            evt2Doc,
            newDoc), IActorT<Spoke>;

    [Name(ToRabbitMq_v1)]
    public class ToRabbitMq(
        IRmqEmitterDriverT<Contract.Start.Payload, MetaB> driver,
        IExchange exchange,
        Evt2Fact<Contract.Start.Payload, MetaB> evt2Fact)
        : EmitterT<Spoke, Contract.Start.Payload, MetaB>(driver, exchange, evt2Fact);

    [Name(FromNATS_v1)]
    public class FromNATS
        : ResponderT<
            Spoke,
            Contract.Start.Payload,
            IHopePipe>
    {
        public FromNATS(
            INATSResponderDriverT<Contract.Start.Payload> driver,
            IExchange exchange,
            IPipeBuilderT<IHopePipe, Contract.Start.Payload> builder)
            : base(driver, exchange, builder)
        {
        }
    }

    public interface IHopePipe
        : IPipeInfoB;

    public interface IRetroPipe
        : IPipeInfoB;

    [Name(FromRabbitMqRetro_v1)]
    public class FromRabbitMqRetro(
        IRmqListenerDriverT<Contract.Start.Payload> driver,
        IExchange exchange,
        IPipeBuilderT<IRetroPipe, Contract.Start.Payload> pipeBuilder)
        : ListenerT<
            Spoke,
            Dummy,
            MetaB,
            Contract.Start.Payload,
            IRetroPipe>(driver, exchange, pipeBuilder);
}