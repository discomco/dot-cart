using Microsoft.Extensions.DependencyInjection;
using NATS.Client;

namespace DotCart.Drivers.NATS;

public static class NATSHandlers
{
}

public static partial class Inject
{
    public static IServiceCollection AddCoreNATS(this IServiceCollection services)
    {
        services?
            //    .AddKubernetes();
            // var container = services.BuildServiceProvider(); 
            // var k8sFact = container.GetService<IKubernetesFactory>();
            // if (!k8sFact.InCluster) 
            //     services.AddStanInfraFromK8S();
            // else
            .AddNatsClient(options =>
            {
                options.User = Config.User;
                options.Password = Config.Password;
                options.Servers = new[]
                {
                    Config.Uri
                };
            }, ServiceLifetime.Singleton);
        return services;
    }


    public static IServiceCollection AddStan(this IServiceCollection services,
        Action<Options> options = null,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        return services
            .AddNatsClient(options, lifetime);
    }
}