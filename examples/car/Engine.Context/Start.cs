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
using Constants = Engine.Contract.Constants;

namespace Engine.Context;

public static class Start
{
    public const string ToRedisDoc_v1 = Behavior.Start.Topics.Evt_v1 + ":to_redis_doc";

    public const string Spoke_v1 = "engine:start:spoke:v1";

    public static IServiceCollection AddStartSpoke(this IServiceCollection services)
    {
        return services
            .AddEngineBehavior()
            .AddStartMappers()
            .AddTransient<IActor<Spoke>, ToRedisDoc>()
            .AddHostedSpokeT<Spoke>()
            .AddDefaultDrivers<Schema.Engine, IEngineSubscriptionInfo>()
            .AddSpokedNATSResponder<Spoke, Contract.Start.Hope, Behavior.Start.Cmd>();
    }

    public interface IToRedisDoc : IActor<Spoke>
    {
    }

    [Name(ToRedisDoc_v1)]
    [DbName(Constants.DocRedisDbName)]
    public class ToRedisDoc : ProjectionT<
        IRedisStore<Schema.Engine>,
        Schema.Engine,
        Behavior.Start.IEvt>, IToRedisDoc
    {
        public ToRedisDoc(IExchange exchange,
            IRedisStore<Schema.Engine> modelStore,
            Evt2State<Schema.Engine, Behavior.Start.IEvt> evt2Doc,
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
}