using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace DotCart.Drivers.Redis;

public static class Inject
{
    public static IServiceCollection AddRedis<TModel>(this IServiceCollection services)
        where TModel : IState
    {
        return services
            .AddTransient(p =>
            {
                var opts = ConfigurationOptions.Parse(Config.ConfigString);
                opts.Ssl = Config.UseSsl;
                opts.Password = Config.Password;
                opts.User = Config.User;
                opts.AllowAdmin = Config.AllowAdmin;
                opts.DefaultDatabase = Convert.ToInt32(DbNameAtt.Get<TModel>());
                return opts;
            })
            .AddTransient<IRedisConnectionFactory, RedisConnectionFactory>();
    }

    public static IServiceCollection AddSingletonRedisConnection<TModel>(this IServiceCollection services)
        where TModel : IState
    {
        return services
            .AddRedis<TModel>()
            .AddSingleton(p =>
            {
                var fact = p.GetRequiredService<IRedisConnectionFactory>();
                return fact.Connect();
            });
    }

    public static IServiceCollection AddTransientRedisConnection<TModel>(this IServiceCollection services)
        where TModel : IState
    {
        return services
            .AddRedis<TModel>()
            .AddTransient(p =>
            {
                var fact = p.GetRequiredService<IRedisConnectionFactory>();
                return fact.Connect();
            });
    }

    public static IServiceCollection AddSingletonRedisDb<TModel>(this IServiceCollection services) 
        where TModel : IState
    {
        return services
            .AddSingletonRedisConnection<TModel>()
            .AddTransient<IModelStore<TModel>, RedisStore<TModel>>()
            .AddTransient<IRedisStore<TModel>, RedisStore<TModel>>()
            .AddSingleton<IRedisDb, RedisDb>();
    }

    public static IServiceCollection AddTransientRedisDb<TModel>(this IServiceCollection services) 
        where TModel : IState
    {
        return services
            .AddTransientRedisConnection<TModel>()
            .AddTransient<IRedisStore<TModel>, RedisStore<TModel>>()
            .AddTransient<IModelStore<TModel>, RedisStore<TModel>>()
            .AddTransient<IRedisDb, RedisDb>();
    }
}

public interface IRedisStore<TModel> : IModelStore<TModel> where TModel : IState
{
}