using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IStoreFactoryT<TDbInfo, TDoc, TID>
    : IStoreFactoryB
    where TDbInfo : IDbInfoB
    where TDoc : IState
    where TID : IID
{
    IDocStoreT<TDoc> Create();
}