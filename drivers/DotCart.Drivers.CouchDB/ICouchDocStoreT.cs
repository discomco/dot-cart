using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Defaults.CouchDb;

namespace DotCart.Drivers.CouchDB;

public interface ICouchDocStoreT<TDbInfo, TDoc, TID>
    : IDocStoreT<TDoc>
    where TDoc : IState
    where TID : IID
    where TDbInfo : ICouchDbInfoB
{
}