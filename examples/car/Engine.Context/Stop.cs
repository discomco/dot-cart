using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
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
                .AddHopeInPipe<IHopeInPipe, Contract.Stop.Payload, EventMeta>()
                .AddTransient<IActorT<Spoke>, ToRedisDoc>()
                .AddTransient<IActorT<Spoke>, ToRedisList>()
                .AddDefaultDrivers<IEngineProjectorInfo, Schema.Engine, Schema.EngineList>()
                .AddNATSResponder<Spoke,
                    FromNATS,
                    Contract.Stop.Payload,
                    EventMeta>()
                .AddRabbitMqEmitter<Spoke,
                    ToRabbitMq,
                    Contract.Stop.Payload,
                    EventMeta>()
                .AddRabbitMqListener<Spoke,
                    FromRabbitMqRetro,
                    Contract.Stop.Payload,
                    Contract.Stop.Dummyload,
                    EventMeta,
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
    [DbName(DbConstants.DocRedisDbName)]
    public class ToRedisDoc
        : ProjectionT<
            IRedisStore<Schema.Engine>,
            Schema.Engine,
            Contract.Stop.Payload, EventMeta>, IActorT<Spoke>
    {
        public ToRedisDoc(
            IExchange exchange,
            IRedisStore<Schema.Engine> docStore,
            Evt2Doc<Schema.Engine, Contract.Stop.Payload, EventMeta> evt2Doc,
            StateCtorT<Schema.Engine> newDoc)
            : base(exchange, docStore, evt2Doc, newDoc)
        {
        }
    }

    [Name(ToRedisList_v1)]
    [DbName(DbConstants.ListRedisDbName)]
    [DocId(IDConstants.EngineListId)]
    public class ToRedisList
        : ProjectionT<
            IRedisStore<Schema.EngineList>,
            Schema.EngineList,
            Contract.Stop.Payload, EventMeta>, IActorT<Spoke>
    {
        public ToRedisList(
            IExchange exchange,
            IRedisStore<Schema.EngineList> docStore,
            Evt2Doc<Schema.EngineList, Contract.Stop.Payload, EventMeta> evt2Doc,
            StateCtorT<Schema.EngineList> newDoc)
            : base(exchange, docStore, evt2Doc, newDoc)
        {
        }
    }

    [Name(ToRabbitMq_v1)]
    public class ToRabbitMq
        : EmitterT<Spoke, Contract.Stop.Payload, EventMeta>
    {
        public ToRabbitMq(
            IRmqEmitterDriverT<Contract.Stop.Payload, EventMeta> driver,
            IExchange exchange,
            Evt2Fact<Contract.Stop.Payload, EventMeta> evt2Fact)
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
        : ListenerT<
            Spoke,
            Contract.Stop.Dummyload,
            EventMeta,
            Contract.Stop.Payload,
            byte[],
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