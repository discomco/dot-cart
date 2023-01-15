using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Defaults.CouchDb;

namespace DotCart.Drivers.CouchDB;

public interface ICouchStoreBuilder<TDbInfo, TDoc, TID>
    : IStoreBuilderT<TDbInfo, TDoc, TID>
    where TDoc : IState
    where TID : IID
    where TDbInfo : ICouchDbInfoB
{
    ICouchDBStore<TDbInfo, TDoc, TID> BuildStore();
}