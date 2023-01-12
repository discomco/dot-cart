using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Actors;
using DotCart.Defaults.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace DotCart.Drivers.RabbitMQ;

public static class Inject
{
    public static IServiceCollection AddSingletonRMq(this IServiceCollection services)
    {
        return services
            .AddSingleton<IConnectionFactory, ConnectionFactory>(p => new ConnectionFactory
            {
                DispatchConsumersAsync = Config.DispatchConsumerAsync,
                VirtualHost = Config.VHost,
                HostName = Config.Host,
                UserName = Config.User,
                Password = Config.Password
            });
    }

    public static IServiceCollection AddRabbitMqEmitterDriverT<TPayload, TMeta>(this IServiceCollection services)
        where TPayload : IPayload
        where TMeta : IMetaB
    {
        return services
            .AddSingletonRMq()
            .AddTransient<IRmqEmitterDriverT<TPayload, TMeta>, RMqEmitterDriverT<TPayload, TMeta>>();
    }

    public static IServiceCollection AddRabbitMqListenerDriverT<TFactPayload, TMeta>(this IServiceCollection services)
        where TFactPayload : IPayload
        where TMeta : IMetaB
    {
        return services
            .AddSingletonRMq()
            .AddTransient<IRmqListenerDriverT<TFactPayload>, RMqListenerDriverT<TFactPayload, TMeta>>();
    }

    public static IServiceCollection AddRabbitMqEmitter<TSpoke, TEmitter, TFactPayload, TMeta>(
        this IServiceCollection services)
        where TFactPayload : IPayload
        where TMeta : IMetaB
        where TEmitter : EmitterT<TSpoke, TFactPayload, TMeta>, IActorT<TSpoke>
        where TSpoke : ISpokeT<TSpoke>
    {
        return services
            .AddRabbitMqEmitterDriverT<TFactPayload, TMeta>()
            .AddTransient<IEmitterT<TSpoke, TFactPayload, TMeta>, TEmitter>()
            .AddTransient<IActorT<TSpoke>, TEmitter>();
    }

    public static IServiceCollection AddRabbitMqListener<
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
            .AddRabbitMqListenerDriverT<TFactPayload, TMeta>()
            .AddTransient<IListenerT<TSpoke, TCmdPayload, TMeta, TFactPayload, TPipeInfo>, TListener>()
            .AddTransient<IActorT<TSpoke>, TListener>();
    }
}