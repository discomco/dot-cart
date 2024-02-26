using DotCart.Core;
using DotCart.Defaults.Redis;
using Engine.Contract;

namespace Engine.Context;

[DbName(DbConstants.RedisListDbName)]
public interface IRedisListDbInfo
    : IRedisDbInfoB
{
}