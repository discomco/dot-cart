using DotCart.Core;
using DotCart.Defaults.CouchDb;
using DotCart.Defaults.Redis;
using Engine.Contract;

namespace Engine.Context;

[DbName(DbConstants.RedisDocDbName)]
public interface IRedisDocDbInfo : IRedisDbInfoB
{
}

[DbName(DbConstants.RedisListDbName)]
public interface IRedisListDbInfo : IRedisDbInfoB
{
}

public interface ICouchDocDbInfo : ICouchDbInfoB
{
}

public interface ICouchListDbInfo : ICouchDbInfoB
{
}