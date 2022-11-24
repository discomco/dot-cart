using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Context.Actors;
using DotCart.Context.Spokes;
using DotCart.Drivers.Default;
using DotCart.Drivers.NATS;
using DotCart.Drivers.Redis;
using Engine.Behavior;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context;

public static class Initialize
{
    public static IServiceCollection AddInitializeSpoke(this IServiceCollection services)
    {
        return services
            .AddEngineBehavior()
            
            .AddTransient<IToRedisDoc, ToRedisDoc>()
            .AddTransient<IActor<Spoke>, ToRedisDoc>()
            
            .AddTransient<Spoke>()
            .AddSingleton<ISpokeBuilder<Spoke>, SpokeBuilder>()
            .AddHostedService(provider =>
            {
                var spokeBuilder = provider.GetRequiredService<ISpokeBuilder<Spoke>>();
                return spokeBuilder.Build();
            })
            .AddDefaultDrivers<Behavior.Engine, IEngineSubscriptionInfo>()
            .AddSpokedNATSResponder<Spoke, Contract.Initialize.Hope, Behavior.Initialize.Cmd>();
    }

    public interface IToRedisDoc : IActor<Spoke>
    {
    }

    internal class ToRedisDoc : ProjectionT<
            IRedisStore<Behavior.Engine>,
            Behavior.Engine,
            Behavior.Initialize.IEvt>,
        IToRedisDoc
    {
        public ToRedisDoc(IExchange exchange,
            IRedisStore<Behavior.Engine> modelStore,
            Evt2State<Behavior.Engine, Behavior.Initialize.IEvt> evt2State) : base(exchange,
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

    public class Spoke : SpokeT<Spoke>, ISpoke
    {
        public Spoke(
            IExchange exchange,
            IProjector projector) : base(exchange, projector)
        {
        }
    }
}