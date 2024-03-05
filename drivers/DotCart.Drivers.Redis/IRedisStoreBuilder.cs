using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;

namespace DotCart.Drivers.Redis;

public interface IRedisStoreFactory<TDbInfo, TDoc, TID>
    : IStoreFactoryT<TDbInfo, TDoc, TID>
    where TDbInfo : IDbInfoB
    where TDoc : IState
    where TID : IID
{
}