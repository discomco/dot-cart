using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
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

public static class Start
{
    public const string ToRedisDoc_v1 = Behavior.Start.Topics.Evt_v1 + ":to_redis_doc:v1";
    public const string ToRedisList_v1 = Behavior.Start.Topics.Evt_v1 + ":to_redis_list:v1";
    
    public const string Spoke_v1 = "engine:start:spoke:v1";

    public static IServiceCollection AddStartSpoke(this IServiceCollection services)
    {
        return services
            .AddEngineBehavior()
            .AddStartACLFuncs()
            .AddStartProjectionFuncs()
            .AddTransient<IActor<Spoke>, ToRedisDoc>()
            .AddHostedSpokeT<Spoke>()
            .AddDefaultDrivers<Schema.Engine, IEngineSubscriptionInfo>()
            .AddDefaultDrivers<Schema.EngineList, IEngineSubscriptionInfo>()
            .AddSpokedNATSResponder<Spoke, Contract.Start.Hope, Behavior.Start.Cmd>();
    }

    public interface IToRedisDoc : IActor<Spoke>
    {
    }

    [Name(ToRedisDoc_v1)]
    [DbName(DbConstants.DocRedisDbName)]
    public class ToRedisDoc : ProjectionT<
        IRedisStore<Schema.Engine>,
        Schema.Engine,
        Behavior.Start.IEvt>, IToRedisDoc
    {
        public ToRedisDoc(IExchange exchange,
            IRedisStore<Schema.Engine> modelStore,
            Evt2Doc<Schema.Engine, Behavior.Start.IEvt> evt2Doc,
            StateCtorT<Schema.Engine> newDoc)
            : base(exchange, modelStore, evt2Doc, newDoc)
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

    [DbName(DbConstants.ListRedisDbName)]
    [DocId(IDConstants.EngineListId)]
    [Name(ToRedisList_v1)]
    public class ToRedisList 
        : ProjectionT<
            IRedisStore<Schema.EngineList>, 
            Schema.EngineList, 
            Behavior.Start.IEvt>, IActor<Spoke>
    {
        public ToRedisList(IExchange exchange,
            IRedisStore<Schema.EngineList> modelStore,
            Evt2Doc<Schema.EngineList, Behavior.Start.IEvt> evt2Doc,
            StateCtorT<Schema.EngineList> newDoc) : base(exchange,
            modelStore,
            evt2Doc,
            newDoc)
        {
        }
    }
}