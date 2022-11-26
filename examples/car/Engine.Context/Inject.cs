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