using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;

namespace DotCart.Drivers.Redis;

public interface IRedisStoreBuilder<TDbInfo, TDoc, TID>
    : IStoreBuilderT<TDbInfo, TDoc, TID>
    where TDbInfo : IDbInfoB
    where TDoc : IState
    where TID : IID
{
}