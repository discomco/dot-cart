using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;

namespace DotCart.Drivers.NATS;

public static class NATSHandlers
{
}

public static class Inject
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
            });
        return services;
    }

    public static IServiceCollection AddStan(this IServiceCollection services,
        Action<Options> options = null,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        return services
            .AddNatsClient(options, lifetime);
    }


    public static IServiceCollection AddNATSResponder<THope, TCmd>(this IServiceCollection services)
        where TCmd : ICmd where THope : IHope
    {
        return services
            .AddCoreNATS()
            .AddSingleton<IResponderDriverT<THope>, NATSResponderDriverT<THope>>()
            .AddSingleton<IResponderT<THope, TCmd>, ResponderT<IResponderDriverT<THope>, THope, TCmd>>();
    }

    public static IServiceCollection AddSpokedNATSResponder<TSpoke, THope, TCmd>(this IServiceCollection services)
        where TCmd : ICmd
        where THope : IHope
    {
        return services
            .AddCoreNATS()
            .AddSingleton<IResponderDriverT<THope>, NATSResponderDriverT<THope>>()
            .AddSingleton<IResponderT<THope, TCmd>, ResponderT<TSpoke, IResponderDriverT<THope>, THope, TCmd>>()
            .AddSingleton<IActor<TSpoke>, ResponderT<TSpoke, IResponderDriverT<THope>, THope, TCmd>>();
    }
}