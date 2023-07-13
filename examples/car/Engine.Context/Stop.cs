using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Context.Actors;
using DotCart.Context.Spokes;
using DotCart.Core;
using DotCart.Defaults;
using DotCart.Defaults.RabbitMq;
using DotCart.Drivers.NATS;
using DotCart.Drivers.RabbitMQ;
using DotCart.Drivers.Redis;
using Engine.Behavior;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context;

public static class Stop
{
    public const string Spoke_v1 = "engine:stop:spoke:v1";

    public const string ToRedisDoc_v1 = Contract.Stop.Topics.Evt_v1 + ":to_redis_doc:v1";
    public const string ToRedisList_v1 = Contract.Stop.Topics.Evt_v1 + ":to_redis_list:v1";

    public const string ToRabbitMq_v1 = Contract.Stop.Topics.Fact_v1 + ":to_rabbit_mq:v1";
    public const string FromRabbitMqRetro_v1 = Contract.Stop.Topics.Fact_v1 + ":from_rabbit_mq_retro:v1";


    public const string FromNATS_v1 = Contract.Stop.Topics.Hope_v1 + ":from_nats:v1";


    public static IServiceCollection AddStopSpoke(this IServiceCollection services)
    {
        return services
                .AddEngineBehavior()
                .AddStopACLFuncs()
                .AddHostedSpokeT<Spoke>()
                .AddHopeInPipe<IHopeInPipe, Contract.Stop.Payload, MetaB>()
                .AddTransient<IActorT<Spoke>, ToRedisDoc>()
                .AddTransient<IActorT<Spoke>, ToRedisList>()
                .AddDotRedis<IRedisListDbInfo, Schema.EngineList, Schema.EngineListID>()
                .AddDotRedis<IRedisDocDbInfo, Schema.Engine, Schema.EngineID>()
                .AddProjectorInfra<IEngineProjectorInfo, Schema.Engine, Schema.EngineList>()
                .AddNATSResponderT<Spoke,
                    FromNATS,
                    Contract.Stop.Payload,
                    MetaB>()
                .AddRabbitMqEmitter<Spoke,
                    ToRabbitMq,
                    Contract.Stop.Payload,
                    MetaB>()
                .AddRabbitMqListener<Spoke,
                    FromRabbitMqRetro,
                    Dummy,
                    MetaB,
                    Contract.Stop.Payload,
                    IRetroInPipe>()
            ;
    }

    [Name(Spoke_v1)]
    public class Spoke : SpokeT<Spoke>
    {
        public Spoke(
            IExchange exchange,
            IProjector projector)
            : base(exchange, projector)
        {
        }
    }


    public interface IToRedisDoc : IActorT<Spoke>
    {
    }

    [Name(ToRedisDoc_v1)]
    [DbName(DbConstants.RedisDocDbName)]
    public class ToRedisDoc
        : ProjectionT<
            IRedisDocDbInfo,
            Schema.Engine,
            Contract.Stop.Payload, MetaB, Schema.EngineID>, IActorT<Spoke>
    {
        public ToRedisDoc(IExchange exchange,
            IStoreBuilderT<IRedisDocDbInfo, Schema.Engine, Schema.EngineID> storeBuilder,
            Evt2Doc<Schema.Engine, Contract.Stop.Payload, MetaB> evt2Doc,
            StateCtorT<Schema.Engine> newDoc) : base(exchange,
            storeBuilder,
            evt2Doc,
            newDoc)
        {
        }
    }

    [Name(ToRedisList_v1)]
    [DbName(DbConstants.RedisListDbName)]
    [DocId(IDConstants.EngineListId)]
    public class ToRedisList
        : ProjectionT<
            IRedisListDbInfo,
            Schema.EngineList,
            Contract.Stop.Payload, MetaB, Schema.EngineListID>, IActorT<Spoke>
    {
        public ToRedisList(IExchange exchange,
            IStoreBuilderT<IRedisListDbInfo, Schema.EngineList, Schema.EngineListID> storeBuilder,
            Evt2Doc<Schema.EngineList, Contract.Stop.Payload, MetaB> evt2Doc,
            StateCtorT<Schema.EngineList> newDoc) : base(exchange,
            storeBuilder,
            evt2Doc,
            newDoc)
        {
        }
    }

    [Name(ToRabbitMq_v1)]
    public class ToRabbitMq
        : EmitterT<Spoke, Contract.Stop.Payload, MetaB>
    {
        public ToRabbitMq(
            IRmqEmitterDriverT<Contract.Stop.Payload, MetaB> driver,
            IExchange exchange,
            Evt2Fact<Contract.Stop.Payload, MetaB> evt2Fact)
            : base(driver, exchange, evt2Fact)
        {
        }
    }

    [Name(FromNATS_v1)]
    public class FromNATS
        : ResponderT<
            Spoke,
            Contract.Stop.Payload,
            IHopeInPipe>
    {
        public FromNATS(
            INATSResponderDriverT<Contract.Stop.Payload> driver,
            IExchange exchange,
            IPipeBuilderT<IHopeInPipe, Contract.Stop.Payload> builder)
            : base(driver, exchange, builder)
        {
        }
    }

    public interface IHopeInPipe : IPipeInfoB
    {
    }

    public interface IRetroInPipe : IPipeInfoB
    {
    }

    [Name(FromRabbitMqRetro_v1)]
    public class FromRabbitMqRetro
        : ListenerT<Spoke,
            Dummy,
            MetaB,
            Contract.Stop.Payload,
            IRetroInPipe>
    {
        public FromRabbitMqRetro(
            IRmqListenerDriverT<Contract.Stop.Payload> driver,
            IExchange exchange,
            IPipeBuilderT<IRetroInPipe, Contract.Stop.Payload> pipeBuilder)
            : base(driver, exchange, pipeBuilder)
        {
        }
    }
}