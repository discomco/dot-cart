using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Spokes;
using DotCart.Core;
using DotCart.Drivers.Default;
using DotCart.Drivers.NATS;
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


    private static readonly Hope2Cmd<Contract.ChangeDetails.Payload, EventMeta>
        _hope2Cmd =
            hope => CmdT<Contract.ChangeDetails.Payload, EventMeta>.New(
                hope.AggId.IDFromIdString(),
                hope.Payload,
                EventMeta.New(
                    NameAtt.Get<IEngineAggregateInfo>(),
                    hope.AggId
                )
            );

    private static readonly Evt2Fact<Contract.ChangeDetails.Payload, EventMeta>
        _evt2Fact =
            evt => FactT<Contract.ChangeDetails.Payload, EventMeta>.New(
                evt.AggregateId,
                evt.Payload,
                evt.Meta
            );

    public static IServiceCollection AddChangeDetailsSpoke(this IServiceCollection services)
    {
        return services
            .AddEngineBehavior()
            .AddChangeDetailsMappers()
            .AddHostedSpokeT<Spoke>()
            .AddTransient<IActor<Spoke>, ToRedisDoc>()
            .AddTransient<IActor<Spoke>, ToRedisList>()
            .AddDefaultDrivers<Schema.Engine, IEngineSubscriptionInfo>()
            .AddDefaultDrivers<Schema.EngineList, IEngineSubscriptionInfo>()
            .AddSpokedNATSResponder<Spoke, Contract.ChangeDetails.Payload, EventMeta>();
    }

    public static IServiceCollection AddChangeDetailsMappers(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _evt2Fact)
            .AddTransient(_ => _hope2Cmd);
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
        Contract.ChangeDetails.Payload, EventMeta>, IActor<Spoke>
    {
        public ToRedisDoc(IExchange exchange,
            IRedisStore<Schema.Engine> docStore,
            Evt2Doc<Schema.Engine, Contract.ChangeDetails.Payload, EventMeta> evt2Doc,
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
        Contract.ChangeDetails.Payload, EventMeta>, IActor<Spoke>
    {
        public ToRedisList(
            IExchange exchange,
            IRedisStore<Schema.EngineList> docStore,
            Evt2Doc<Schema.EngineList,Contract.ChangeDetails.Payload, EventMeta> evt2Doc,
            StateCtorT<Schema.EngineList> newDoc)
            : base(exchange, docStore, evt2Doc, newDoc)
        {
        }
    }
}