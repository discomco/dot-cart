﻿namespace DotCart.Drivers.Redis;

/// <summary>
///     Can be used to easily create strongly-typed RedisObjects with a specified key name pattern.
/// </summary>
/// <typeparam name="T"></typeparam>
public class KeyTemplate<T>
    where T : RedisObject
{
    private readonly IRedisDb _db;
    private readonly string _keyPattern;

    internal KeyTemplate(IRedisDb db, string keyPattern)
    {
        _db = db;
        _keyPattern = keyPattern;
    }

    public T GetKey(object arg1)
    {
        var s = string.Format(_keyPattern, arg1);
        return _db.GetKey<T>(s);
    }

    public T GetKey(params object[] args)
    {
        var s = string.Format(_keyPattern, args);
        return _db.GetKey<T>(s);
    }
}