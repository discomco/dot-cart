using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;

namespace DotCart.Drivers.Redis;

public interface IRedisStoreT<TDoc>
    : IDocStoreT<TDoc> where TDoc : IState
{
}