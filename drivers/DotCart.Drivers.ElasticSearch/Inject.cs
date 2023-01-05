using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace DotCart.Drivers.ElasticSearch;

public static class Inject
{
    public static IServiceCollection AddElasticSearch(this IServiceCollection services,
        IConnectionSettingsValues forceConnectionSettings = null)
    {
        if (forceConnectionSettings != null)
            services.AddSingleton<IConnectionSettingsValues>(_ => forceConnectionSettings);
        else
            services.AddSingleton<IConnectionSettingsValues>(new ConnectionSettings(new Uri(Config.Url)));
        return services
            .AddTransient<IElasticLowLevelClient, ElasticLowLevelClient>()
            .AddTransient<IElasticClient, ElasticClient>();
    }
}