using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Defaults.CouchDb;

namespace DotCart.Drivers.CouchDB;

public class CouchStoreFactoryT<TDbInfo, TDoc, TID>
    : ICouchStoreFactoryT<TDbInfo, TDoc, TID>, ICouchAdminFactoryT<TDbInfo>
    where TDoc : IState
    where TID : IID
    where TDbInfo : ICouchDbInfoB
{
    private readonly IMyCouchFactory _factory;

    public CouchStoreFactoryT(IMyCouchFactory factory)
    {
        _factory = factory;
    }

    public ICouchAdminStore<TDbInfo> BuildAdmin()
    {
        return new CouchAdminStoreT<TDbInfo>(_factory);
    }

    public IDocStoreT<TDoc> Create()
    {
        return new CouchDocStoreT<TDbInfo, TDoc, TID>(_factory);
    }
}