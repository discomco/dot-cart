using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using Microsoft.AspNetCore.Routing;
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

    public static IServiceCollection AddRabbitMqEmitterDriverT<TPayload,TMeta>(this IServiceCollection services)
        where TPayload : IPayload
        where TMeta : IEventMeta
    {
        return services
            .AddTransient<IEmitterDriverT<TPayload,TMeta>, RMqEmitterDriverT<TPayload, TMeta>>();
    }

    public static IServiceCollection AddRabbitMqListenerDriverT<TFactPayload, TMeta>(this IServiceCollection services)
        where TFactPayload : IPayload 
        where TMeta : IEventMeta
    {
        return services
            .AddTransient<IListenerDriverT<TFactPayload, byte[]>, RMqListenerDriverT<TFactPayload, TMeta>>();
    }
}