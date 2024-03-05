using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace DotCart.Drivers.Redis;

public static class Inject
{
    private static readonly Func<ConfigurationOptions>
        redisOptionsFromConfig = () =>
        {
            var options = ConfigurationOptions.Parse(Config.ConfigString);
            options.Ssl = Config.UseSsl;
            options.Password = Config.Password;
            options.User = Config.User;
            options.AllowAdmin = Config.AllowAdmin;
            return options;
        };


    public static IServiceCollection AddRedisConnectionFactory<TDbInfo, TDoc>(this IServiceCollection services,
        Func<ConfigurationOptions>? configureOptions = null)
        where TDoc : IState
        where TDbInfo : IDbInfoB
    {
        configureOptions ??= redisOptionsFromConfig;
        return services
            .AddTransient(_ => configureOptions)
            .AddSingleton<IRedisConnectionFactory<TDbInfo, TDoc>, RedisConnectionFactory<TDbInfo, TDoc>>();
    }


    public static IServiceCollection AddDotRedis<TDbInfo, TDoc, TID>(this IServiceCollection services,
        Func<ConfigurationOptions>? configOptions = null)
        where TDoc : IState
        where TDbInfo :
        IDbInfoB
        where TID : IID
    {
        configOptions ??= redisOptionsFromConfig;
        services.AddRedisConnectionFactory<TDbInfo, TDoc>(configOptions);
        services.TryAddSingleton<
            IRedisStoreFactory<TDbInfo, TDoc, TID>,
            RedisStoreFactory<TDbInfo, TDoc, TID>>();
        services.TryAddSingleton<
            IStoreFactoryT<TDbInfo, TDoc, TID>,
            RedisStoreFactory<TDbInfo, TDoc, TID>>();
        return services;
    }
}