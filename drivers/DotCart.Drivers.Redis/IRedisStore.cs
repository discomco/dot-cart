using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;

namespace DotCart.Drivers.Redis;

public interface IRedisStore<TDoc>
    : IDocStore<TDoc> where TDoc : IState
{
}