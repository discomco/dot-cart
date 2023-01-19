using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Context.Actors;
using DotCart.Context.Spokes;
using DotCart.Core;
using DotCart.Defaults;
using DotCart.Drivers.CouchDB;
using DotCart.Drivers.NATS;
using DotCart.Drivers.Redis;
using Engine.Behavior;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context;

public static class ChangeRpm
{
    public const string ToCouchDoc_v1 = Contract.ChangeRpm.Topics.Evt_v1 + ":to_couch_doc:v1";
    public const string ToRedisDoc_v1 = Contract.ChangeRpm.Topics.Evt_v1 + ":to_redis_doc:v1";
    public const string ToRedisList_v1 = Contract.ChangeRpm.Topics.Evt_v1 + ":to_redis_list:v1";
    public const string FromNATS_v1 = Contract.ChangeRpm.Topics.Hope_v1 + ":from_nats:v1";

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
            .AddNATSResponder<Spoke, FromNATS, Contract.ChangeRpm.Payload, MetaB>();
    }

    [Name(ToRedisDoc_v1)]
    [DbName(DbConstants.RedisDocDbName)]
    public class ToRedisDoc : ProjectionT<
        IRedisDocDbInfo,
        Schema.Engine,
        Contract.ChangeRpm.Payload,
        MetaB,
        Schema.EngineID>, IActorT<Spoke>
    {
        public ToRedisDoc(IExchange exchange,
            IStoreBuilderT<IRedisDocDbInfo, Schema.Engine, Schema.EngineID> storeBuilder,
            Evt2Doc<Schema.Engine, Contract.ChangeRpm.Payload, MetaB> evt2Doc,
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

    [DocId(IDConstants.EngineListId)]
    [DbName(DbConstants.RedisListDbName)]
    [Name(ToRedisList_v1)]
    public class ToRedisList : ProjectionT<
        IRedisListDbInfo,
        Schema.EngineList,
        Contract.ChangeRpm.Payload, MetaB, Schema.EngineListID>, IActorT<Spoke>
    {
        public ToRedisList(IExchange exchange,
            IStoreBuilderT<IRedisListDbInfo, Schema.EngineList, Schema.EngineListID> storeBuilder,
            Evt2Doc<Schema.EngineList, Contract.ChangeRpm.Payload, MetaB> evt2Doc,
            StateCtorT<Schema.EngineList> newDoc) : base(exchange,
            storeBuilder,
            evt2Doc,
            newDoc)
        {
        }
    }

    [Name(FromNATS_v1)]
    public class FromNATS
        : ResponderT<
            Spoke,
            Contract.ChangeRpm.Payload,
            IHopePipe>
    {
        public FromNATS(
            INATSResponderDriverT<Contract.ChangeRpm.Payload> driver,
            IExchange exchange,
            IPipeBuilderT<IHopePipe, Contract.ChangeRpm.Payload> builder)
            : base(driver, exchange, builder)
        {
        }
    }

    public interface IHopePipe : IPipeInfoB
    {
    }

    

    [Name(ToCouchDoc_v1)]
    [DbName(DbConstants.CouchDocDbName)]
    public class ToCouchDoc
    : ProjectionT<Context.ICouchDocDbInfo,
        Schema.Engine,
        Contract.ChangeRpm.Payload,
        MetaB,
        Schema.EngineID>, IActorT<Spoke>
    {
        public ToCouchDoc(IExchange exchange,
            IStoreBuilderT<ICouchDocDbInfo, Schema.Engine, Schema.EngineID> storeBuilder,
            Evt2Doc<Schema.Engine, Contract.ChangeRpm.Payload, MetaB> evt2Doc,
            StateCtorT<Schema.Engine> newDoc) : base(exchange,
            storeBuilder,
            evt2Doc,
            newDoc)
        {
        }        
    }
}