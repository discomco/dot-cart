using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
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

    public static IServiceCollection AddRabbitMqEmitterDriverT<TIFact, TPayload>(this IServiceCollection services)
        where TIFact : IFactB
        where TPayload : IPayload
    {
        return services
            .AddTransient<IEmitterDriverT<TPayload>, RMqEmitterDriverT<TIFact, TPayload>>();
    }

    public static IServiceCollection AddRabbitMqListenerDriverT<TIFact, TPayload>(this IServiceCollection services)
        where TIFact : IFactB
        where TPayload : IPayload
    {
        return services
            .AddTransient<IListenerDriverT<TIFact, byte[]>, RMqListenerDriverT<TIFact, TPayload>>();
    }
}