using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Actors;
using DotCart.Context.Spokes;
using DotCart.Core;
using DotCart.Defaults;
using DotCart.Drivers.NATS;
using DotCart.Drivers.Redis;
using Engine.Behavior;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context;

public static class ChangeRpm
{
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
            .AddTransient<IActorT<Spoke>, ToRedisDoc>()
            .AddTransient<IActorT<Spoke>, ToRedisList>()
            .AddDefaultDrivers<IEngineProjectorInfo, Schema.Engine, Schema.EngineList>()
            .AddHopeInPipe<IHopePipe, Contract.ChangeRpm.Payload, Meta>()
            .AddNATSResponder<Spoke, FromNATS, Contract.ChangeRpm.Payload, Meta>();
    }

    [Name(ToRedisDoc_v1)]
    [DbName(DbConstants.DocRedisDbName)]
    public class ToRedisDoc : ProjectionT<
        IRedisStore<Schema.Engine>,
        Schema.Engine,
        Contract.ChangeRpm.Payload, Meta>, IActorT<Spoke>
    {
        public ToRedisDoc(IExchange exchange,
            IRedisStore<Schema.Engine> docStore,
            Evt2Doc<Schema.Engine, Contract.ChangeRpm.Payload, Meta> evt2Doc,
            StateCtorT<Schema.Engine> newDoc)
            : base(exchange, docStore, evt2Doc, newDoc)
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
    [DbName(DbConstants.ListRedisDbName)]
    [Name(ToRedisList_v1)]
    public class ToRedisList : ProjectionT<
        IRedisStore<Schema.EngineList>,
        Schema.EngineList,
        Contract.ChangeRpm.Payload, Meta>, IActorT<Spoke>
    {
        public ToRedisList(
            IExchange exchange,
            IRedisStore<Schema.EngineList> docStore,
            Evt2Doc<Schema.EngineList, Contract.ChangeRpm.Payload, Meta> evt2Doc,
            StateCtorT<Schema.EngineList> newDoc) : base(exchange, docStore, evt2Doc, newDoc)
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
}