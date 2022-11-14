using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace DotCart.Drivers.Polly;

public static class Inject
{
    public static IServiceCollection AddPolly(this IServiceCollection services)
    {
        return services.AddTransient(_ => Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(Config.MaxRetries,
                times => TimeSpan.FromMilliseconds(times * 100)));
    }
}