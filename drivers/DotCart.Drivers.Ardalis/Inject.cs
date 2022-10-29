using DotCart.Drivers.Microsoft;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Drivers.Ardalis;

public static class Inject
{
    public static IServiceCollection AddArdalisDrivers(this IServiceCollection services)
    {
        return services
            .AddMicrosoftDrivers();
    }
}