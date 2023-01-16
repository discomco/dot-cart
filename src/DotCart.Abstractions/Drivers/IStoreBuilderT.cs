using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IStoreBuilderB {}

public interface IStoreBuilderT<TDbInfo, TDoc, TID> 
    : IStoreBuilderB
    where TDbInfo : IDbInfoB
    where TDoc : IState
    where TID : IID
{
    IDocStore<TDoc> Build();
}

public interface IAdminStoreBuilderT<TDbInfo> 
    : IStoreBuilderB
    where TDbInfo : IDbInfoB
{}