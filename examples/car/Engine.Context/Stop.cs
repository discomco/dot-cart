using Engine.Behavior;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context;

public static class Stop
{
    public static IServiceCollection AddStopSpoke(this IServiceCollection services)
    {
        return services
            .AddEngineBehavior()
            .AddStopMappers();
    }
}