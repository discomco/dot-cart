using DotCart.Abstractions.Schema;
using DotCart.Core;
using StackExchange.Redis;

namespace DotCart.Drivers.Redis;

public class RedisConnectionFactory<TDoc> : IRedisConnectionFactory<TDoc> 
    where TDoc : IState
{
    private ConfigurationOptions _options;

    public RedisConnectionFactory(ConfigurationOptions options)
    {
        _options = options;
    }

    public IConnectionMultiplexer Connect(ConfigurationOptions options = null)
    {
        if (options != null) 
            _options = options;
        _options.DefaultDatabase = Convert.ToInt32(DbNameAtt.Get<TDoc>());
        return ConnectionMultiplexer.Connect(_options);
    }
}

public interface IRedisConnectionFactory<TDoc> 
    where TDoc : IState
{
    IConnectionMultiplexer Connect(ConfigurationOptions options = null);
}