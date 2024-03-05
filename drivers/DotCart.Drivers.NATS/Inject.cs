using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Actors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NATS.Client;

namespace DotCart.Drivers.NATS;

public static class Inject
{
    public static IServiceCollection AddCoreNATS(this IServiceCollection services,
        Action<Options>? configureOptions = null,
        ServiceLifetime connectionServiceLifeTime = ServiceLifetime.Transient)
    {
        if (configureOptions != null)
        {
            services.TryAddSingleton(configureOptions);
            return services
                .AddNatsClient(
                    configureOptions,
                    connectionServiceLifeTime
                );
        }

        services.TryAddSingleton<Action<Options>>(_ =>
            options =>
            {
                options.Timeout = 5000;
                options.AllowReconnect = true;
                options.MaxReconnect = 10;
                options.User = Config.User;
                options.Password = Config.Password;
                options.Servers = new[] { Config.Uri };
            });
        var optionsAction = services
            .BuildServiceProvider()
            .GetRequiredService<Action<Options>>();
        return services
            .AddNatsClient(optionsAction, connectionServiceLifeTime);
    }


    // public static IServiceCollection AddCoreNATSOld(this IServiceCollection services)
    // {
    //     services
    //         //    .AddKubernetes();
    //         // var container = services.BuildServiceProvider(); 
    //         // var k8sFact = container.GetService<IKubernetesFactory>();
    //         // if (!k8sFact.InCluster) 
    //         //     services.AddStanInfraFromK8S();
    //         // else
    //         .TryAddSingleton<Action<Options>>(_ => options =>
    //         {
    //             options.Timeout = 5000;
    //             options.AllowReconnect = true;
    //             options.MaxReconnect = 10;
    //             options.User = Config.User;
    //             options.Password = Config.Password;
    //             options.Servers = new[]
    //             {
    //                 Config.Uri
    //             };
    //         });
    //     var optionsAction = services
    //         .BuildServiceProvider()
    //         .GetRequiredService<Action<Options>>();
    //     services
    //         .AddNatsClient(optionsAction);
    //     return services;
    // }

    public static IServiceCollection AddStan(this IServiceCollection services,
        Action<Options> options = null,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        return services
            .AddNatsClient(options, lifetime);
    }


    public static IServiceCollection AddNATSResponderT<TSpoke, TResponder, TPayload, TMeta>(
        this IServiceCollection services)
        where TPayload : IPayload
        where TMeta : IMetaB
        where TResponder : class, IResponderT<TPayload>, IActorT<TSpoke>
    {
        return services
            .AddNATSResponderDriverT<TPayload>()
            .AddSingleton<IResponderT<TPayload>, TResponder>()
            .AddSingleton<IActorT<TSpoke>, TResponder>();
    }

    public static IServiceCollection AddNATSListenerDriverT<TFactPayload, TMeta>(this IServiceCollection services,
        Action<Options>? configureOptions = null)
        where TFactPayload : IPayload
        where TMeta : IMetaB
    {
        return services
            .AddCoreNATS(configureOptions)
            .AddTransient<INATSListenerDriverT<TFactPayload>, NATSListenerDriverT<TFactPayload, TMeta>>();
    }

    public static IServiceCollection AddNATSListenerT<
        TSpoke,
        TListener,
        TCmdPayload,
        TMeta,
        TFactPayload,
        TPipeInfo>(
        this IServiceCollection services)
        where TMeta : IMetaB
        where TFactPayload : IPayload
        where TListener : ListenerT<TSpoke, TCmdPayload, TMeta, TFactPayload, TPipeInfo>
        where TSpoke : ISpokeT<TSpoke>
        where TPipeInfo : IPipeInfoB
        where TCmdPayload : IPayload
    {
        return services
            .AddPipeBuilder<TPipeInfo, TFactPayload>()
            .AddNATSListenerDriverT<TFactPayload, TMeta>()
            .AddTransient<IListenerT<TSpoke, TCmdPayload, TMeta, TFactPayload, TPipeInfo>, TListener>()
            .AddTransient<IActorT<TSpoke>, TListener>();
    }

    public static IServiceCollection AddNATSResponderDriverT<THopePayload>(this IServiceCollection services,
        Action<Options>? configureOptions = null)
        where THopePayload : IPayload
    {
        return services
            .AddCoreNATS(configureOptions)
            .AddTransient<INATSResponderDriverT<THopePayload>, NATSResponderDriverT<THopePayload>>();
    }


    public static IServiceCollection AddNATSRequesterT<TPayload>(this IServiceCollection services)
        where TPayload : IPayload
    {
        return services
            .AddTransient<INATSRequesterT<TPayload>, NATSRequesterT<TPayload>>();
    }

    public static IServiceCollection AddNATSListener<TFactPayload>(this IServiceCollection services,
        ProcessFactAsync<TFactPayload> processFact)
        where TFactPayload : IPayload
    {
        return services
            .AddTransient(_ => processFact)
            .AddHostedService<NATSListener<TFactPayload>>();
    }

    public static IServiceCollection AddNATSEmitter<TPayload, TMeta>(this IServiceCollection services)
        where TPayload : IPayload
        where TMeta : IMetaB
    {
        return services
            .AddCoreNATS()
            .AddTransient<INATSEmitter<TPayload, TMeta>, NATSEmitter<TPayload, TMeta>>();
    }
}