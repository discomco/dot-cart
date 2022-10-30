using DotCart.Effects;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Drivers.InMem;

public static class Inject
{
    public static IServiceCollection AddMemEventStore(this IServiceCollection services)
    {
        return services
            .AddSingleton<IMemEventStore, MemEventStore>();
    }
}