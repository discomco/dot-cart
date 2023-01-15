using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IStoreBuilderT<TDbInfo, TDoc, TID>
    where TDbInfo : IDbInfoB
    where TDoc : IState
    where TID : IID
{
    IDocStore<TDoc> Build();
}