using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Actors;
using DotCart.Core;
using DotCart.Defaults;
using DotCart.Defaults.RabbitMq;
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
                IRetroPipe>();
    }

    public interface IToRedisDoc : IActorT<Spoke>
    {
    }

    [Name(ToRedisDoc_v1)]
    [DbName(DbConstants.RedisDocDbName)]
    public class ToRedisDoc : ProjectionT<
        IRedisDocDbInfo,
        Schema.Engine,
        Contract.Start.Payload, MetaB, Schema.EngineID>, IToRedisDoc
    {
        public ToRedisDoc(IExchange exchange,
            IStoreBuilderT<IRedisDocDbInfo, Schema.Engine, Schema.EngineID> storeBuilder,
            Evt2Doc<Schema.Engine, Contract.Start.Payload, MetaB> evt2Doc,
            StateCtorT<Schema.Engine> newDoc) : base(exchange,
            storeBuilder,
            evt2Doc,
            newDoc)
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

    [DbName(DbConstants.RedisListDbName)]
    [DocId(IDConstants.EngineListId)]
    [Name(ToRedisList_v1)]
    public class ToRedisList
        : ProjectionT<
            IRedisListDbInfo,
            Schema.EngineList,
            Contract.Start.Payload,
            MetaB, Schema.EngineListID>, IActorT<Spoke>
    {
        public ToRedisList(IExchange exchange,
            IStoreBuilderT<IRedisListDbInfo, Schema.EngineList, Schema.EngineListID> storeBuilder,
            Evt2Doc<Schema.EngineList, Contract.Start.Payload, MetaB> evt2Doc,
            StateCtorT<Schema.EngineList> newDoc) : base(exchange,
            storeBuilder,
            evt2Doc,
            newDoc)
        {
        }
    }

    [Name(ToRabbitMq_v1)]
    public class ToRabbitMq
        : EmitterT<Spoke, Contract.Start.Payload, MetaB>
    {
        public ToRabbitMq(
            IRmqEmitterDriverT<Contract.Start.Payload, MetaB> driver,
            IExchange exchange,
            Evt2Fact<Contract.Start.Payload, MetaB> evt2Fact) : base(driver, exchange, evt2Fact)
        {
        }
    }

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

    public interface IHopePipe : IPipeInfoB
    {
    }

    public interface IRetroPipe : IPipeInfoB
    {
    }

    [Name(FromRabbitMqRetro_v1)]
    public class FromRabbitMqRetro
        : ListenerT<
            Spoke,
            Dummy,
            MetaB,
            Contract.Start.Payload,
            IRetroPipe>
    {
        public FromRabbitMqRetro(
            IRmqListenerDriverT<Contract.Start.Payload> driver,
            IExchange exchange,
            IPipeBuilderT<IRetroPipe, Contract.Start.Payload> pipeBuilder)
            : base(driver, exchange, pipeBuilder)
        {
        }
    }
}