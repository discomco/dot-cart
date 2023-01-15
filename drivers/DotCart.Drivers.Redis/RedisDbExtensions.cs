using StackExchange.Redis;

namespace DotCart.Drivers.Redis;

public static class RedisDbExtensions
{
    public static bool IsConnected(this ISimpleRedisDb redisDb, string keyName)
    {
        if (redisDb == null) throw new ArgumentNullException(nameof(redisDb));
        return redisDb.DB.IsConnected(keyName);
    }

    public static Task<bool> KeyExists(this ISimpleRedisDb redisDb, string keyName, bool useKeyNameSpace = true)
    {
        if (redisDb == null) throw new ArgumentNullException(nameof(redisDb));
        var fullKeyName = useKeyNameSpace ? $"{redisDb.KeyNameSpace}:{keyName}" : keyName;
        return redisDb.DB.KeyExistsAsync(fullKeyName);
    }

    public static Task<long> KeyExists(this ISimpleRedisDb redisDb, IEnumerable<string> keyNames,
        bool useKeyNameSpace = true)
    {
        if (redisDb == null)
            throw new ArgumentNullException(nameof(redisDb));
        var keys = keyNames.Select(k => useKeyNameSpace ? $"{redisDb.KeyNameSpace}:{k}" : k).Cast<RedisKey>()
            .ToArray();
        return redisDb.DB.KeyExistsAsync(keys);
    }

    public static Task<bool> DeleteKey(this ISimpleRedisDb redisDb, string keyName, bool useKeyNameSpace = true)
    {
        if (redisDb == null) throw new ArgumentNullException(nameof(redisDb));
        var fullKeyName = useKeyNameSpace ? $"{redisDb.KeyNameSpace}:{keyName}" : keyName;
        return redisDb.DB.KeyDeleteAsync(fullKeyName);
    }

    public static Task<long> DeleteKey(this ISimpleRedisDb redisDb, IEnumerable<string> keyNames,
        bool useKeyNameSpace = true)
    {
        if (redisDb == null) throw new ArgumentNullException(nameof(redisDb));
        var keys = keyNames.Select(k => useKeyNameSpace ? $"{redisDb.KeyNameSpace}:{k}" : k).Cast<RedisKey>()
            .ToArray();
        return redisDb.DB.KeyDeleteAsync(keys);
    }
}