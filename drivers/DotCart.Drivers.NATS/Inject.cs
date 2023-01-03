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
        services
            //    .AddKubernetes();
            // var container = services.BuildServiceProvider(); 
            // var k8sFact = container.GetService<IKubernetesFactory>();
            // if (!k8sFact.InCluster) 
            //     services.AddStanInfraFromK8S();
            // else
            .AddNatsClient(options =>
            {
                options.AllowReconnect = true;
                options.MaxReconnect = 10;
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


    public static IServiceCollection AddNATSResponder<TPayload, TMeta>(this IServiceCollection services)
        where TPayload : IPayload
        where TMeta : IEventMeta
    {
        return services
            .AddCoreNATS()
            .AddSingleton<IResponderDriverT<TPayload>, NATSResponderDriverT<TPayload>>()
            .AddSingleton<IResponderT<TPayload>, ResponderT<TPayload, TMeta>>();
    }

    public static IServiceCollection AddSpokedNATSResponder<TSpoke, TPayload, TMeta>(
        this IServiceCollection services)
        where TPayload : IPayload
        where TMeta : IEventMeta
    {
        return services
            .AddCoreNATS()
            .AddSingleton<IResponderDriverT<TPayload>, NATSResponderDriverT<TPayload>>()
            .AddSingleton<IResponderT<TPayload>, ResponderT<TSpoke, TPayload, TMeta>>()
            .AddSingleton<IActor<TSpoke>, ResponderT<TSpoke, TPayload, TMeta>>();
    }
}