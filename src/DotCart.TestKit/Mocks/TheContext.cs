using DotCart.Abstractions.Actors;
using DotCart.Core;
using DotCart.Defaults.CouchDb;
using DotCart.Defaults.Redis;

namespace DotCart.TestKit.Mocks;

public static class TheContext
{
    public interface IPipeInfo : IPipeInfoB
    {
    }

    [DbName(TheConstants.CouchDocDbName)]
    public interface ICouchDocDbInfo : ICouchDbInfoB
    {
    }

    [DbName(TheConstants.CouchListDbName)]
    public interface ICouchListDbInfo : ICouchDbInfoB
    {
    }

    [DbName(TheConstants.RedisDocDbName)]
    public interface IRedisDocDbInfo : IRedisDbInfoB
    {
    }

    [DbName(TheConstants.RedisListDbName)]
    public interface IRedisListDbInfo : IRedisDbInfoB
    {
    }
}