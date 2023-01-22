using DotCart.Core;
using DotCart.Defaults.Redis;
using DotCart.TestKit.Mocks;

namespace DotCart.Drivers.Redis.Tests;

public static class TheContext
{
    [DbName(TheConstants.RedisDocDbName)]
    public interface IRedisDocDbInfo : IRedisDbInfoB
    {
    }

    [DbName(TheConstants.RedisListDbName)]
    public interface IRedisListDbInfo : IRedisDbInfoB
    {
    }
}