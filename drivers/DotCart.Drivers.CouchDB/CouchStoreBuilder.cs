using CouchDB.Driver;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Defaults.CouchDb;
using DotCart.Drivers.CouchDB.Internal.Interfaces;

namespace DotCart.Drivers.CouchDB;

public class CouchStoreBuilder<TDbInfo, TDoc, TID>
    : ICouchStoreBuilder<TDbInfo, TDoc, TID>
    where TDoc : IState
    where TID : IID
    where TDbInfo : ICouchDbInfoB
{
    private readonly ICouchClient _couchClient;
    private readonly ICouchServer _couchServer;

    public CouchStoreBuilder(
        ICouchServer couchServer,
        ICouchClient couchClient)
    {
        _couchServer = couchServer;
        _couchClient = couchClient;
    }

    public ICouchDBStore<TDbInfo, TDoc, TID> BuildStore()
    {
        return new CouchStore<TDbInfo, TDoc, TID>(_couchClient, _couchServer);
    }

    public IDocStore<TDoc> Build()
    {
        return new CouchStore<TDbInfo, TDoc, TID>(_couchClient, _couchServer);
    }
}