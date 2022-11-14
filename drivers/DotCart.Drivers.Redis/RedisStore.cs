using DotCart.Contract.Schemas;
using DotCart.Core;
using StackExchange.Redis;

namespace DotCart.Drivers.Redis;

public class RedisStore<TState> : IRedisStore<TState> where TState : IState
{
    private readonly object _delMutex = new();
    private readonly IRedisDb _redisDb;

    private readonly object _setMutex = new();

    private object _getMutex = new();

    public RedisStore(
        IRedisDb redisDb
    )
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

    public Task<TState> SetAsync(string id, TState doc, CancellationToken cancellationToken = default)
    {
        return Task.Run<TState>(() =>
        {
            lock (_setMutex)
            {
                var isSet = _redisDb.Database.StringSet(new RedisKey(id), new RedisValue(doc.ToJson()));
                return isSet ? doc : default;
            }
        }, cancellationToken);
    }


    public Task<TState> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            lock (_delMutex)
            {
                var res = _redisDb.Database.StringGetDelete(new RedisKey(id));
                if (res.IsNullOrEmpty) return default;
                var json = res.ToString();
                return json.FromJson<TState>();
            }
        }, cancellationToken);
    }

    public Task<bool> Exists(string id, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
                _redisDb.Database.KeyExists(new RedisKey(id))
            , cancellationToken);
    }

    public Task<TState?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return Task.Run<TState>(() =>
        {
            lock (_setMutex)
            {
                var redisValue = _redisDb.Database.StringGet(new RedisKey(id));
                var json = redisValue.ToString();
                return redisValue.HasValue
                    ? json.FromJson<TState>()
                    : default;
            }
        }, cancellationToken);
    }

    public Task<bool> HasData(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask DisposeAsync()
    {
        return _redisDb.DisposeAsync();
    }

    public Task CloseAsync(bool allowCommandsToComplete)
    {
        return _redisDb.CloseAsync(allowCommandsToComplete);
    }
}