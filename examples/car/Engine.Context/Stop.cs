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

public static class Stop
{
    public static IServiceCollection AddStopSpoke(this IServiceCollection services)
    {
        return services
            .AddHostedSpokeT<Spoke>()
            .AddSpokedNATSResponder<Spoke, Contract.Stop.Hope, Behavior.Stop.Cmd>()
            .AddDefaultDrivers<Behavior.Engine, IEngineSubscriptionInfo>()
            .AddTransient<IActor<Spoke>, ToRedisDoc>()
            .AddEngineBehavior()
            .AddStopMappers();
    }


    public const string Spoke_v1 = "engine:stop:spoke:v1";

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

    public const string ToRedisDoc_v1 = Behavior.Stop.Topics.Evt_v1 + ":to_redis_doc";
    
    [Name(ToRedisDoc_v1)]
    [DbName(Constants.DocRedisDbName)]
    public class ToRedisDoc
        : ProjectionT<
            IRedisStore<Behavior.Engine>,
            Behavior.Engine,
            Behavior.Stop.IEvt>, IActor<Spoke>
    {
        public ToRedisDoc(
            IExchange exchange,
            IRedisStore<Behavior.Engine> modelStore,
            Evt2State<Behavior.Engine, Behavior.Stop.IEvt> evt2State,
            StateCtorT<Behavior.Engine> newDoc)
            : base(exchange, modelStore, evt2State, newDoc)
        {
        }
    }
}