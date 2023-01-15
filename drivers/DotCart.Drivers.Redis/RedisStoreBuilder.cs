using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;

namespace DotCart.Drivers.Redis;

public class RedisStoreBuilder<TDbInfo, TDoc, TID>
    : IRedisStoreBuilder<TDbInfo, TDoc, TID>
    where TDoc : IState
    where TDbInfo : IDbInfoB
    where TID : IID
{
    private readonly IRedisConnectionFactory<TDbInfo, TDoc> _connectionFactory;

    public RedisStoreBuilder(IRedisConnectionFactory<TDbInfo, TDoc> connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IDocStore<TDoc> Build()
    {
        var db = RedisDbT<TDbInfo, TDoc>.New(_connectionFactory);
        return RedisStore<TDbInfo, TDoc>.New(db);
    }
}