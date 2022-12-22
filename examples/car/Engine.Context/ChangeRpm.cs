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

public static class ChangeRpm
{
    public const string ToRedisDoc_v1 = "engine:changed_rpm:to_redis_doc:v1";


    public const string Spoke_v1 = "engine:change_rpm:spoke:v1";

    public static IServiceCollection AddChangeRpmSpoke(this IServiceCollection services)
    {
        return services
            .AddEngineBehavior()
            .AddChangeRpmMappers()
            .AddHostedSpokeT<Spoke,SpokeBuilder>()
            // .AddTransient<Spoke>()
            // .AddSingleton<ISpokeBuilder<Spoke>, SpokeBuilder>()
            // .AddHostedService(provider =>
            // {
            //     var spokeBuilder = provider.GetRequiredService<ISpokeBuilder<Spoke>>();
            //     return spokeBuilder.Build();
            // })
            .AddDefaultDrivers<Behavior.Engine, IEngineSubscriptionInfo>()
            .AddTransient<IActor<Spoke>, ToRedisDoc>()
            .AddSpokedNATSResponder<Spoke, Contract.ChangeRpm.Hope, Behavior.ChangeRpm.Cmd>();
    }


    [Name(ToRedisDoc_v1)]
    [DbName("3")]
    public class ToRedisDoc : ProjectionT<
        IRedisStore<Behavior.Engine>,
        Behavior.Engine,
        Behavior.ChangeRpm.IEvt>, IActor<Spoke>
    {
        public ToRedisDoc(IExchange exchange,
            IRedisStore<Behavior.Engine> modelStore,
            Evt2State<Behavior.Engine, Behavior.ChangeRpm.IEvt> evt2State,
            StateCtorT<Behavior.Engine> newDoc)
            : base(exchange, modelStore, evt2State, newDoc)
        {
        }
    }

    public class SpokeBuilder : SpokeBuilderT<Spoke>
    {
        public SpokeBuilder(
            Spoke spoke,
            IEnumerable<IActor<Spoke>> actors) : base(spoke, actors)
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