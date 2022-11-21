using DotCart.Abstractions.Drivers;
using DotCart.Context.Effects;
using DotCart.Drivers.EventStoreDB;
using DotCart.Drivers.Redis;
using Engine.Context.Common.Drivers;
using Engine.Context.Initialize;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context;

public static class Inject
{
    public static IServiceCollection AddRedisStoreDriver(this IServiceCollection services)
    {
        return services
            .AddTransient<IModelStore<Common.Schema.Engine>, RedisStore<Common.Schema.Engine>>();
    }

    public static IServiceCollection AddEngineContext(this IServiceCollection services)
    {
        return services
            .AddConfiguredESDBClients()
            .AddESDBStore()
            .AddSingletonProjector<IEngineSubscriptionInfo>()
            .AddInitializeSpoke();
    }
}