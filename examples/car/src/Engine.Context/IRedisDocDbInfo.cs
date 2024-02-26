using DotCart.Core;
using DotCart.Defaults.Redis;
using Engine.Contract;

namespace Engine.Context;

[DbName(DbConstants.RedisDocDbName)]
public interface IRedisDocDbInfo
    : IRedisDbInfoB
{
}