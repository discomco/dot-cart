﻿using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using StackExchange.Redis;

namespace DotCart.Drivers.Redis;

public interface ISimpleRedisDb
{
    public string KeyNameSpace { get; }
    IDatabaseAsync DB { get; }
}

public interface IRedisDb : ISimpleRedisDb, IClose, IDisposable, IAsyncDisposable, ICloseAsync
{
    IDatabase Database { get; }
    IList<string> TrackedKeys { get; }
    IList<RedisObject> TrackedObjects { get; }
    T GetKey<T>(string keyName) where T : RedisObject;
    T AddToContainer<T>(T obj) where T : RedisObject;
    RedisObject GetKey(Type keyType, string keyName);
    KeyTemplate<T> GetKeyTemplate<T>(string keyNamePattern) where T : RedisObject;
    RedisTransactionDb CreateTransaction(object state = null);
    BatchRedisDb CreateBatch(object state = null);
    Task DeleteTrackedKeys();
}

public interface IRedisDbT<TDbInfo, TDoc> : IRedisDb
    where TDoc : IState
{
}

public class RedisDbT<TDbInfo, TDoc>
    : IRedisDbT<TDbInfo, TDoc>
    where TDoc : IState
    where TDbInfo : IDbInfoB
{
    private readonly IConnectionMultiplexer _connection;
    private readonly IRedisConnectionFactory<TDbInfo, TDoc> _connFact;
    private readonly Dictionary<string, RedisObject> _trackedObjects = new();
    private readonly bool _trackObjects;

    private RedisDbT(IRedisConnectionFactory<TDbInfo, TDoc> connFact)
    {
        // TODO: Implement TDbInfo for Redis
        KeyNameSpace = "";
        _trackObjects = false;
        _connFact = connFact;
        _connection = _connFact.Connect();
        Database = _connection.GetDatabase();
    }

    public IDatabase Database { get; }

    /// <summary>
    ///     Returns the list of key names tracked within this container.
    /// </summary>
    public IList<string> TrackedKeys
    {
        get { return _trackedObjects.Select(p => p.Key).ToList(); }
    }

    /// <summary>
    ///     Returns the list of RedisObjects tracked within this container.
    /// </summary>
    public IList<RedisObject> TrackedObjects
    {
        get { return _trackedObjects.Select(p => p.Value).ToList(); }
    }

    IDatabaseAsync ISimpleRedisDb.DB => Database;

    public string KeyNameSpace { get; }


    /// <summary>
    ///     Add a strongly-typed RedisObject to the container.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public T AddToContainer<T>(T obj)
        where T : RedisObject
    {
        obj.Db = this;
        obj.KeyName = string.IsNullOrWhiteSpace(KeyNameSpace)
            ? $"{obj.BaseKeyName}"
            : $"{KeyNameSpace}:{obj.BaseKeyName}";

        if (!_trackObjects) return obj;
        if (!TrackedKeys.Contains(obj.KeyName))
            _trackedObjects.Add(obj.KeyName, obj);
        return obj;
    }

    /// <summary>
    ///     Returns a strongly-typed RedisObject for the key name.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="keyName"></param>
    /// <returns></returns>
    /// <remarks>
    ///     <para>This will not create the key in Redis, but the key might already exist in Redis independent of this object.</para>
    ///     <para>
    ///         If the container was created with the 'TrackObjects' option (the default)
    ///         and the RedisObject was already created then that object is returned.  Otherwise a new instance is created.
    ///     </para>
    /// </remarks>
    public T GetKey<T>(string keyName) where T : RedisObject
    {
        return GetKey(typeof(T), keyName) as T;
    }

    /// <summary>
    ///     Returns a strongly-typed RedisObject for the key name.
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="keyName"></param>
    /// <returns></returns>
    /// <remarks>
    ///     <para>This will not create the key in Redis, but the key might already exist in Redis independent of this object.</para>
    ///     <para>
    ///         If the container was created with the 'TrackObjects' option (the default)
    ///         and the RedisObject was already created then that object is returned.  Otherwise a new instance is created.
    ///     </para>
    /// </remarks>
    public RedisObject GetKey(Type keyType, string keyName)
    {
        RedisObject obj;
        var fullKeyName = $"{KeyNameSpace}:{keyName}";
        if (_trackObjects)
            if (_trackedObjects.TryGetValue(fullKeyName, out obj))
                return obj;
        var instance = Activator.CreateInstance(keyType, keyName) as RedisObject;
        AddToContainer(instance);
        return instance;
    }

    /// <summary>
    ///     Returns a KeyTemplate for the specified strongly-typed RedisObject and key pattern.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="keyNamePattern"></param>
    /// <returns></returns>
    public KeyTemplate<T> GetKeyTemplate<T>(string keyNamePattern) where T : RedisObject
    {
        return new KeyTemplate<T>(this, keyNamePattern);
    }

    /// <summary>
    ///     Returns a RedisTransactionProxy for the Redis transaction.
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public RedisTransactionDb CreateTransaction(object state = null)
    {
        return new RedisTransactionDb(Database.CreateTransaction(state), KeyNameSpace);
    }

    /// <summary>
    ///     Returns a RedisBatchProxy for the Redis batch.
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public BatchRedisDb CreateBatch(object state = null)
    {
        return new BatchRedisDb(Database.CreateBatch(state), KeyNameSpace);
    }

    /// <summary>
    ///     Delete all keys from Redis which are tracked by this container.
    /// </summary>
    /// <returns></returns>
    public async Task DeleteTrackedKeys()
    {
        var keys = TrackedKeys.Select(k => (RedisKey)k).ToArray();
        await Database.KeyDeleteAsync(keys);
        _trackedObjects.Clear();
    }

    public void Dispose()
    {
        _connection.Close();
    }

    public ValueTask DisposeAsync()
    {
        return _connection.DisposeAsync();
    }

    public void Close()
    {
        _connection.Close();
    }

    public Task CloseAsync(bool allowCommandsToComplete)
    {
        return _connection.CloseAsync(allowCommandsToComplete);
    }


    public static RedisDbT<TDbInfo, TDoc> New(IRedisConnectionFactory<TDbInfo, TDoc> connFact)
    {
        return new RedisDbT<TDbInfo, TDoc>(connFact);
    }
}