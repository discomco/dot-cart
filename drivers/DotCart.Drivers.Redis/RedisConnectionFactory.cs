using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using StackExchange.Redis;

namespace DotCart.Drivers.Redis;

public class RedisConnectionFactory<TDbInfo, TDoc>
    : IRedisConnectionFactory<TDbInfo, TDoc>
    where TDoc : IState
    where TDbInfo : IDbInfoB
{
    private readonly ConfigurationOptions _options;

    public RedisConnectionFactory(Func<ConfigurationOptions> options)
    {
        _options = options();
    }

    public IConnectionMultiplexer Connect()
    {
        _options.DefaultDatabase = Convert.ToInt32(DbNameAtt.Get<TDbInfo>());
        return ConnectionMultiplexer.Connect(_options);
    }
}

public interface IRedisConnectionFactory<TDbInfo, TDoc>
    where TDoc : IState
    where TDbInfo : IDbInfoB
{
    IConnectionMultiplexer Connect();
}