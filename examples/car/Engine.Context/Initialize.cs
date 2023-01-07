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

public static class Initialize
{
    public const string ToRedisDoc_v1 = Contract.Initialize.Topics.Evt_v1 + ":to_redis_doc:v1";
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
            .AddHopeInPipe<IHopePipe, Contract.Initialize.Payload, Meta>()
            .AddHostedSpokeT<Spoke>()
            .AddNATSResponder<Spoke, FromNATS, Contract.Initialize.Payload, Meta>()
            .AddTransient<IActorT<Spoke>, ToRedisDoc>()
            .AddTransient<IActorT<Spoke>, ToRedisList>()
            .AddDefaultDrivers<IEngineProjectorInfo, Schema.Engine, Schema.EngineList>()
            .AddRabbitMqEmitter<Spoke, ToRabbitMq, Contract.Initialize.Payload, Meta>()
            .AddRabbitMqListener<Spoke,
                FromRabbitMqRetro,
                Contract.Initialize.Payload,
                Contract.Initialize.Dummyload,
                Meta,
                IRetroPipe>();
    }


    [Name(ToRedisDoc_v1)]
    [DbName(DbConstants.DocRedisDbName)]
    public class ToRedisDoc : ProjectionT<
            IRedisStore<Schema.Engine>,
            Schema.Engine,
            Contract.Initialize.Payload,
            Meta
        >,
        IActorT<Spoke>
    {
        public ToRedisDoc(
            IExchange exchange,
            IRedisStore<Schema.Engine> docStore,
            Evt2Doc<Schema.Engine, Contract.Initialize.Payload, Meta> evt2Doc,
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
        Meta>, IActorT<Spoke>
    {
        public ToRedisList(
            IExchange exchange,
            IRedisStore<Schema.EngineList> docStore,
            Evt2Doc<Schema.EngineList, Contract.Initialize.Payload, Meta> evt2Doc,
            StateCtorT<Schema.EngineList> newDoc)
            : base(exchange, docStore, evt2Doc, newDoc)
        {
        }
    }

    [Name(ToRabbitMq_v1)]
    public class ToRabbitMq
        : EmitterT<Spoke, Contract.Initialize.Payload, Meta>
    {
        public ToRabbitMq(
            IRmqEmitterDriverT<Contract.Initialize.Payload, Meta> driver,
            IExchange exchange,
            Evt2Fact<Contract.Initialize.Payload, Meta> evt2Fact) : base(driver,
            exchange,
            evt2Fact)
        {
        }
    }

    [Name(FromNATS_v1)]
    public class FromNATS
        : ResponderT<
            Spoke,
            Contract.Initialize.Payload,
            IHopePipe>
    {
        public FromNATS(
            INATSResponderDriverT<Contract.Initialize.Payload> driver,
            IExchange exchange,
            IPipeBuilderT<IHopePipe, Contract.Initialize.Payload> builder)
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
    public class FromRabbitMqRetro :
        ListenerT<Spoke,
            Contract.Initialize.Dummyload,
            Meta,
            Contract.Initialize.Payload,
            byte[],
            IRetroPipe>
    {
        public FromRabbitMqRetro(
            IRmqListenerDriverT<Contract.Initialize.Payload> driver,
            IExchange exchange,
            IPipeBuilderT<IRetroPipe, Contract.Initialize.Payload> pipeBuilder)
            : base(driver, exchange, pipeBuilder)
        {
        }
    }
}