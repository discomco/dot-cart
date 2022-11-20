using DotCart.Abstractions.Actors;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Drivers.Mediator;

public static partial class Inject
{
    public static IServiceCollection AddSingletonExchange(this IServiceCollection services)
    {
        return services
            .AddSingleton<IExchange, Exchange>();
    }
}