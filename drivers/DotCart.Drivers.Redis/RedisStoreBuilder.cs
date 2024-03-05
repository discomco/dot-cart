using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;

namespace DotCart.Drivers.Redis;

public class RedisStoreFactory<TDbInfo, TDoc, TID>
    : IRedisStoreFactory<TDbInfo, TDoc, TID>
    where TDoc : IState
    where TDbInfo : IDbInfoB
    where TID : IID
{
    private readonly IRedisConnectionFactory<TDbInfo, TDoc> _connectionFactory;

    public RedisStoreFactory(IRedisConnectionFactory<TDbInfo, TDoc> connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IDocStoreT<TDoc> Create()
    {
        var db = RedisDbT<TDbInfo, TDoc>.New(_connectionFactory);
        return RedisStoreT<TDbInfo, TDoc>.New(db);
    }
}