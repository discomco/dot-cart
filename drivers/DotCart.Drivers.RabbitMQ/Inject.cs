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
        where TMeta : IMeta
    {
        return services
            .AddSingletonRMq()
            .AddTransient<IRmqEmitterDriverT<TPayload, TMeta>, RMqEmitterDriverT<TPayload, TMeta>>();
    }

    public static IServiceCollection AddRabbitMqListenerDriverT<TFactPayload, TMeta>(this IServiceCollection services)
        where TFactPayload : IPayload
        where TMeta : IMeta
    {
        return services
            .AddSingletonRMq()
            .AddTransient<IRmqListenerDriverT<TFactPayload>, RMqListenerDriverT<TFactPayload, TMeta>>();
    }

    public static IServiceCollection AddRabbitMqEmitter<TSpoke, TEmitter, TPayload, TMeta>(
        this IServiceCollection services)
        where TPayload : IPayload
        where TMeta : IMeta
        where TEmitter : EmitterT<TSpoke, TPayload, TMeta>, IActorT<TSpoke>
        where TSpoke : ISpokeT<TSpoke>
    {
        return services
            .AddRabbitMqEmitterDriverT<TPayload, TMeta>()
            .AddTransient<IActorT<TSpoke>, TEmitter>();
    }

    public static IServiceCollection AddRabbitMqListener<
        TSpoke,
        TListener,
        TFactPayload,
        TCmdPayload,
        TMeta,
        TPipeInfo>(
        this IServiceCollection services)
        where TMeta : IMeta
        where TFactPayload : IPayload
        where TListener : ListenerT<TSpoke, TCmdPayload, TMeta, TFactPayload, byte[], TPipeInfo>
        where TSpoke : ISpokeT<TSpoke>
        where TPipeInfo : IPipeInfoB
        where TCmdPayload : IPayload
    {
        return services
            .AddPipeBuilder<TPipeInfo, TFactPayload>()
            .AddRabbitMqListenerDriverT<TFactPayload, TMeta>()
            .AddTransient<IActorT<TSpoke>, TListener>();
    }
}