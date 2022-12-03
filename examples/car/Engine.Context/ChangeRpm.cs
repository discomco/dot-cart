using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
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
    public const string ToRedisDocProjectionName = "engine:changed_rpm:to_redis_doc";


    public const string SpokeName = "engine:change_rpm:spoke";

    public static IServiceCollection AddChangeRpmSpoke(this IServiceCollection services)
    {
        return services
            .AddEngineBehavior()
            .AddTransient<IActor<Spoke>, ToRedisDoc>()
            .AddTransient<Spoke>()
            .AddSingleton<ISpokeBuilder<Spoke>, SpokeBuilder>()
            .AddHostedService(provider =>
            {
                var spokeBuilder = provider.GetRequiredService<ISpokeBuilder<Spoke>>();
                return spokeBuilder.Build();
            })
            .AddDefaultDrivers<Behavior.Engine, IEngineSubscriptionInfo>()
            .AddSpokedNATSResponder<Spoke, Contract.ChangeRpm.Hope, Behavior.ChangeRpm.Cmd>();
    }


    [Name(ToRedisDocProjectionName)]
    public class ToRedisDoc : ProjectionT<
        IRedisStore<Behavior.Engine>,
        Behavior.Engine,
        Behavior.ChangeRpm.IEvt>, IActor<Spoke>
    {
        public ToRedisDoc(IExchange exchange,
            IRedisStore<Behavior.Engine> modelStore,
            Evt2State<Behavior.Engine, Behavior.ChangeRpm.IEvt> evt2State) : base(exchange,
            modelStore,
            evt2State)
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