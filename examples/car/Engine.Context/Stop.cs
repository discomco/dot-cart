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

public static class Stop
{
    public const string Spoke_v1 = "engine:stop:spoke:v1";

    public const string ToRedisDoc_v1 = Behavior.Stop.Topics.Evt_v1 + ":to_redis_doc";

    public static IServiceCollection AddStopSpoke(this IServiceCollection services)
    {
        return services
            .AddHostedSpokeT<Spoke>()
            .AddSpokedNATSResponder<Spoke, Contract.Stop.Hope, Behavior.Stop.Cmd>()
            .AddDefaultDrivers<Schema.Engine, IEngineSubscriptionInfo>()
            .AddTransient<IActor<Spoke>, ToRedisDoc>()
            .AddEngineBehavior()
            .AddStopMappers();
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


    public interface IToRedisDoc : IActor<Spoke>
    {
    }

    [Name(ToRedisDoc_v1)]
    [DbName(Constants.DocRedisDbName)]
    public class ToRedisDoc
        : ProjectionT<
            IRedisStore<Schema.Engine>,
            Schema.Engine,
            Behavior.Stop.IEvt>, IActor<Spoke>
    {
        public ToRedisDoc(
            IExchange exchange,
            IRedisStore<Schema.Engine> modelStore,
            Evt2State<Schema.Engine, Behavior.Stop.IEvt> evt2Doc,
            StateCtorT<Schema.Engine> newDoc)
            : base(exchange, modelStore, evt2Doc, newDoc)
        {
        }
    }
}