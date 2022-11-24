using DotCart.Context.Actors;
using DotCart.Drivers.Default;
using DotCart.Drivers.EventStoreDB;
using DotCart.Drivers.Redis;
using Engine.Behavior;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context;

public static class Inject
{
    public static IServiceCollection AddCartwheel(this IServiceCollection services)
    {
        return services
            .AddInitializeSpoke()
            .AddChangeRpmSpoke()
            .AddStartSpoke();
    }

    
    
    
    
}