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

public static class Initialize
{
    public const string ToRedisDocName = "engine:initialized:to_redis_doc:v1";
    public const string SpokeName = "engine:initialize:spoke";

    public static IServiceCollection AddInitializeSpoke(this IServiceCollection services)
    {
        return services
            .AddEngineBehavior()
            .AddInitializeMappers()
            .AddTransient<IActor<Spoke>, ToRedisDoc>()
            .AddSpokedNATSResponder<Spoke, Contract.Initialize.Hope, Behavior.Initialize.Cmd>()
            .AddTransient<Spoke>()
            .AddSingleton<ISpokeBuilder<Spoke>, SpokeBuilder>()
            .AddHostedService(provider =>
            {
                var spokeBuilder = provider.GetRequiredService<ISpokeBuilder<Spoke>>();
                return spokeBuilder.Build();
            })
            .AddDefaultDrivers<Behavior.Engine, IEngineSubscriptionInfo>();
    }


    public interface IToRedisDoc : IActor<Spoke>
    {
    }

    [Name(ToRedisDocName)]
    public class ToRedisDoc : ProjectionT<
            IRedisStore<Behavior.Engine>,
            Behavior.Engine,
            Behavior.Initialize.Evt>,
        IToRedisDoc
    {
        public ToRedisDoc(IExchange exchange,
            IRedisStore<Behavior.Engine> modelStore,
            Evt2State<Behavior.Engine, Behavior.Initialize.Evt> evt2State) : base(exchange,
            modelStore,
            evt2State)
        {
        }
    }


    public class SpokeBuilder : SpokeBuilderT<Spoke>
    {
        public SpokeBuilder(
            Spoke spoke,
            IEnumerable<IActor<Spoke>> actors) : base(
            spoke,
            actors)
        {
        }
    }

    public interface ISpoke : ISpokeT<Spoke>
    {
    }


    [Name(SpokeName)]
    public class Spoke : SpokeT<Spoke>, ISpoke
    {
        public Spoke(
            IExchange exchange,
            IProjector projector) : base(exchange, projector)
        {
        }
    }
}