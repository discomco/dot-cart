using DotCart.Abstractions.Schema;
using DotCart.Core;
using StackExchange.Redis;

namespace DotCart.Drivers.Redis;

public class RedisStore<TDbInfo, TDoc> : IRedisStore<TDoc>
    where TDoc : IState
{
    private readonly object _delMutex = new();
    private readonly IRedisDbT<TDbInfo, TDoc> _redisDb;

    private readonly object _setMutex = new();

    private object _getMutex = new();

    protected RedisStore(IRedisDbT<TDbInfo, TDoc> redisDb)
    {
        _redisDb = redisDb;
    }


    public void Close()
    {
        _redisDb.Close();
    }

    public void Dispose()
    {
        _redisDb.Dispose();
    }

    public Task<TDoc> SetAsync(string id, TDoc doc, CancellationToken cancellationToken = default)
    {
        return Task.Run<TDoc>(() =>
        {
            lock (_setMutex)
            {
                var isSet = _redisDb.Database.StringSet(new RedisKey(id), new RedisValue(doc.ToJson()));
                return isSet ? doc : default;
            }
        }, cancellationToken);
    }


    public Task<TDoc> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            lock (_delMutex)
            {
                var res = _redisDb.Database.StringGetDelete(new RedisKey(id));
                if (res.IsNullOrEmpty) return default;
                var json = res.ToString();
                return json.FromJson<TDoc>();
            }
        }, cancellationToken);
    }

    public Task<bool> Exists(string id, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
                _redisDb.Database.KeyExists(new RedisKey(id))
            , cancellationToken);
    }

    public Task<TDoc?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return Task.Run<TDoc>(() =>
        {
            lock (_setMutex)
            {
                var redisValue = _redisDb.Database.StringGet(new RedisKey(id));
                var json = redisValue.ToString();
                return redisValue.HasValue
                    ? json.FromJson<TDoc>()
                    : default;
            }
        }, cancellationToken);
    }

    public async Task<bool> HasData(CancellationToken cancellationToken = default)
    {
        return _redisDb.Database != null;
    }

    public ValueTask DisposeAsync()
    {
        return _redisDb.DisposeAsync();
    }

    public Task CloseAsync(bool allowCommandsToComplete)
    {
        return _redisDb.CloseAsync(allowCommandsToComplete);
    }


    public static RedisStore<TDbInfo, TDoc> New(IRedisDbT<TDbInfo, TDoc> redisDb)
    {
        return new RedisStore<TDbInfo, TDoc>(redisDb);
    }
}