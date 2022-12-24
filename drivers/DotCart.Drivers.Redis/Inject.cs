using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace DotCart.Drivers.Redis;

public static class Inject
{
    public static IServiceCollection AddRedis<TDoc>(this IServiceCollection services)
        where TDoc : IState
    {
        return services
            .AddTransient(p =>
            {
                var opts = ConfigurationOptions.Parse(Config.ConfigString);
                opts.Ssl = Config.UseSsl;
                opts.Password = Config.Password;
                opts.User = Config.User;
                opts.AllowAdmin = Config.AllowAdmin;
                return opts;
            })
            .AddSingleton<IRedisConnectionFactory<TDoc>, RedisConnectionFactory<TDoc>>();
    }

    public static IServiceCollection AddSingletonRedisConnection<TDoc>(this IServiceCollection services)
        where TDoc : IState
    {
        return services
            .AddRedis<TDoc>()
            .AddSingleton(p =>
            {
                var fact = p.GetRequiredService<IRedisConnectionFactory<TDoc>>();
                return fact.Connect();
            });
    }

    public static IServiceCollection AddTransientRedisConnection<TDoc>(this IServiceCollection services)
        where TDoc : IState
    {
        return services
            .AddRedis<TDoc>()
            .AddTransient(p =>
            {
                var fact = p.GetRequiredService<IRedisConnectionFactory<TDoc>>();
                return fact.Connect();
            });
    }

    public static IServiceCollection AddSingletonRedisDb<TDoc>(this IServiceCollection services)
        where TDoc : IState
    {
        return services
            .AddSingletonRedisConnection<TDoc>()
            .AddTransient<IDocStore<TDoc>, RedisStore<TDoc>>()
            .AddTransient<IRedisStore<TDoc>, RedisStore<TDoc>>()
            .AddSingleton<IRedisDbT<TDoc>, RedisDbT<TDoc>>();
    }

    public static IServiceCollection AddTransientRedisDb<TDoc>(this IServiceCollection services)
        where TDoc : IState
    {
        return services
            .AddTransientRedisConnection<TDoc>()
            .AddTransient<IRedisStore<TDoc>, RedisStore<TDoc>>()
            .AddTransient<IDocStore<TDoc>, RedisStore<TDoc>>()
            .AddTransient<IRedisDbT<TDoc>, RedisDbT<TDoc>>();
    }
}

public interface IRedisStore<TDoc> 
    : IDocStore<TDoc> where TDoc : IState
{
}