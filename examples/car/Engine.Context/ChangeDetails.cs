using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;
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

public static class ChangeDetails
{
    public const string Spoke_v1 = "engine:change_details:spoke:v1";


    public const string ToRedisDoc_v1 = Contract.ChangeDetails.Topics.Evt_v1 + ":to_redis_doc:v1";
    public const string ToRedisList_v1 = Contract.ChangeDetails.Topics.Evt_v1 + ":to_redis_list:v1";
    public const string ToRabbitMq_v1 = Contract.ChangeDetails.Topics.Fact_v1 + ":to_rabbit_mq:v1";
    public const string FromRabbitMqRetro_v1 = Contract.ChangeDetails.Topics.Fact_v1 + ":from_rabbit_mq_retro:v1";

    public const string FromNATS_v1 = Contract.ChangeDetails.Topics.Hope_v1 + ":from_nats:v1";


    public static IServiceCollection AddChangeDetailsSpoke(this IServiceCollection services)
    {
        return services
            .AddEngineBehavior()
            .AddChangeDetailsACLFuncs()
            .AddHopeInPipe<IHopePipe, Contract.ChangeDetails.Payload, MetaB>()
            .AddHostedSpokeT<Spoke>()
            .AddTransient<IActorT<Spoke>, ToRedisDoc>()
            .AddTransient<IActorT<Spoke>, ToRedisList>()
            .AddDefaultDrivers<IEngineProjectorInfo, Schema.Engine, Schema.EngineList>()
            .AddNATSResponder<Spoke,
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
    public class Spoke : SpokeT<Spoke>
    {
        public Spoke(
            IExchange exchange,
            IProjector projector)
            : base(exchange, projector)
        {
        }
    }

    [Name(ToRedisDoc_v1)]
    [DbName(DbConstants.DocRedisDbName)]
    public class ToRedisDoc : ProjectionT<
        IRedisStore<Schema.Engine>,
        Schema.Engine,
        Contract.ChangeDetails.Payload, MetaB>, IActorT<Spoke>
    {
        public ToRedisDoc(IExchange exchange,
            IRedisStore<Schema.Engine> docStore,
            Evt2Doc<Schema.Engine, Contract.ChangeDetails.Payload, MetaB> evt2Doc,
            StateCtorT<Schema.Engine> newDoc) : base(exchange,
            docStore,
            evt2Doc,
            newDoc)
        {
        }
    }

    [DbName(DbConstants.ListRedisDbName)]
    [Name(ToRedisList_v1)]
    [DocId(IDConstants.EngineListId)]
    public class ToRedisList : ProjectionT<
        IRedisStore<Schema.EngineList>,
        Schema.EngineList,
        Contract.ChangeDetails.Payload, MetaB>, IActorT<Spoke>
    {
        public ToRedisList(
            IExchange exchange,
            IRedisStore<Schema.EngineList> docStore,
            Evt2Doc<Schema.EngineList, Contract.ChangeDetails.Payload, MetaB> evt2Doc,
            StateCtorT<Schema.EngineList> newDoc)
            : base(exchange, docStore, evt2Doc, newDoc)
        {
        }
    }

    [Name(ToRabbitMq_v1)]
    public class ToRabbitMq
        : EmitterT<
            Spoke,
            Contract.ChangeDetails.Payload,
            MetaB>
    {
        public ToRabbitMq(
            IRmqEmitterDriverT<Contract.ChangeDetails.Payload, MetaB> driver,
            IExchange exchange,
            Evt2Fact<Contract.ChangeDetails.Payload, MetaB> evt2Fact) : base(driver,
            exchange,
            evt2Fact)
        {
        }
    }


    [Name(FromNATS_v1)]
    public class FromNATS
        : ResponderT<
            Spoke,
            Contract.ChangeDetails.Payload,
            IHopePipe>
    {
        public FromNATS(
            INATSResponderDriverT<Contract.ChangeDetails.Payload> driver,
            IExchange exchange,
            IPipeBuilderT<IHopePipe, Contract.ChangeDetails.Payload> builder)
            : base(driver, exchange, builder)
        {
        }
    }

    [Name(FromRabbitMqRetro_v1)]
    public class FromRabbitMqRetro
        : ListenerT<
            Spoke,
            Dummy,
            MetaB,
            Contract.ChangeDetails.Payload,
            IRetroPipe>
    {
        public FromRabbitMqRetro(
            IRmqListenerDriverT<Contract.ChangeDetails.Payload> driver,
            IExchange exchange,
            IPipeBuilderT<IRetroPipe, Contract.ChangeDetails.Payload> pipeBuilder)
            : base(driver, exchange, pipeBuilder)
        {
        }
    }

    public interface IHopePipe : IPipeInfoB
    {
    }

    public interface IRetroPipe : IPipeInfoB
    {
    }
}