using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Context.Actors;
using DotCart.Drivers.EventStoreDB;
using DotCart.Drivers.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Drivers.Default;

public static class Defaults
{
    public static IServiceCollection AddDefaultDrivers<TDoc, TInfo>(this IServiceCollection services)
        where TDoc : IState
        where TInfo : ISubscriptionInfoB
    {
        return services
            .AddESDBStore()
            .AddCmdHandler()
            .AddTransientRedisDb<TDoc>()
            .AddSingletonProjector<TInfo>();
    }
}