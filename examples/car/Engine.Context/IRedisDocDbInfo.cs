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

[DbName(DbConstants.CouchDocDbName)]
public interface ICouchDocDbInfo : ICouchDbInfoB
{
}

[DbName(DbConstants.CouchListDbName)]
public interface ICouchListDbInfo : ICouchDbInfoB
{
}