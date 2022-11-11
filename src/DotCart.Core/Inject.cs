using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Core;

public static class Inject
{
    public static IServiceCollection AddYaml(this IServiceCollection services)
    {
        return services
            .AddTransient<IYamlSerializer, YamlSerializer>()
            .AddTransient<IYamlDeserializer, YamlDeserializer>();
    }
}