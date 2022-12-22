using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.Context.Spokes;
using DotCart.Core;
using DotCart.Drivers.Default;
using DotCart.Drivers.NATS;
using DotCart.Drivers.Redis;
using Engine.Behavior;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context;

public static class Start
{
    public const string ToRedisDocName_v1 = Behavior.Start.Topics.Evt_v1 + ":to_redis_doc";

    public const string SpokeName = "engine:start:spoke";

    public static IServiceCollection AddStartSpoke(this IServiceCollection services)
    {
        return services
            .AddEngineBehavior()
            .AddStartMappers()
            .AddTransient<IActor<Spoke>, ToRedisDoc>()
            .AddHostedSpokeT<Spoke>()
            .AddDefaultDrivers<Behavior.Engine, IEngineSubscriptionInfo>()
            .AddSpokedNATSResponder<Spoke, Contract.Start.Hope, Behavior.Start.Cmd>();
    }

    public interface IToRedisDoc : IActor<Spoke>
    {
    }

    [Name(ToRedisDocName_v1)]
    public class ToRedisDoc : ProjectionT<
        IRedisStore<Behavior.Engine>,
        Behavior.Engine,
        Behavior.Start.IEvt>, IToRedisDoc
    {
        public ToRedisDoc(IExchange exchange,
            IRedisStore<Behavior.Engine> modelStore,
            Evt2State<Behavior.Engine, Behavior.Start.IEvt> evt2State,
            StateCtorT<Behavior.Engine> newDoc)
            : base(exchange, modelStore, evt2State, newDoc)
        {
        }
    }

    [Name(SpokeName)]
    public class Spoke : SpokeT<Spoke>
    {
        public Spoke(
            IExchange exchange,
            IProjector projector) : base(exchange, projector)
        {
        }
    }
}