using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Drivers.EventStoreDB;


public static class Inject
{
    public static IServiceCollection AddEventStoreDBDrivers(this IServiceCollection services)
    {
        return services;
    }

}
