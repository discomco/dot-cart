using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace DotCart.Drivers.ElasticSearch;

public static class Inject
{
    public static IServiceCollection AddElasticSearch(this IServiceCollection services)
    {
        return services
            .AddSingleton<IConnectionSettingsValues>(new ConnectionSettings(new Uri(Config.Url)))
            .AddTransient<IElasticClient, ElasticClient>();
    }
}