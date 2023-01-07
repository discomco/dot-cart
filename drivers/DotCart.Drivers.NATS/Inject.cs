using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
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
            .AddSingleton<Action<Options>>(_ => options =>
            {
                options.Timeout = 5000;
                options.AllowReconnect = true;
                options.MaxReconnect = 10;
                options.User = Config.User;
                options.Password = Config.Password;
                options.Servers = new[]
                {
                    Config.Uri
                };
            })
            .AddNatsClient();
        // .AddNatsClient(options =>
        // {
        //     options.AllowReconnect = true;
        //     options.MaxReconnect = 10;
        //     options.User = Config.User;
        //     options.Password = Config.Password;
        //     options.Servers = new[]
        //     {
        //         Config.Uri
        //     };
        // });
        return services;
    }

    public static IServiceCollection AddStan(this IServiceCollection services,
        Action<Options> options = null,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        return services
            .AddNatsClient(options, lifetime);
    }


    public static IServiceCollection AddNATSResponder<TSpoke, TResponder, TPayload, TMeta>(
        this IServiceCollection services)
        where TPayload : IPayload
        where TMeta : IEventMeta
        where TResponder : class, IResponderT<TPayload>, IActorT<TSpoke>
    {
        return services
            .AddCoreNATS()
            .AddSingleton<INATSResponderDriverT<TPayload>, NATSResponderDriverT<TPayload>>()
            .AddSingleton<IActorT<TSpoke>, TResponder>();
    }
}