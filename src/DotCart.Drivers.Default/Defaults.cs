using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Context.Actors;
using DotCart.Drivers.EventStoreDB;
using DotCart.Drivers.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Drivers.Default;


public static class Defaults
{
    public static IServiceCollection AddDefaultDrivers<TModel, TInfo>(this IServiceCollection services) 
        where TModel : IState 
        where TInfo : ISubscriptionInfo
    {
        return services
            .AddESDBStore()
            .AddCmdHandler()
            .AddTransientRedisDb<TModel>()
            .AddSingletonProjector<TInfo>();
    }
}
