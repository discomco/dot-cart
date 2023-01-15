using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Defaults.CouchDb;

namespace DotCart.Drivers.CouchDB;

public interface ICouchDBStore<TDbInfo, TDoc, TID>
    : IDocStore<TDoc>
    where TDoc : IState
    where TID : IID
    where TDbInfo : ICouchDbInfoB
{
}