using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Defaults.CouchDb;


namespace DotCart.Drivers.CouchDB;

public class CouchStoreBuilderT<TDbInfo, TDoc, TID>
    : ICouchStoreBuilderT<TDbInfo, TDoc, TID>, ICouchAdminBuilderT<TDbInfo>
    where TDoc : IState
    where TID : IID
    where TDbInfo : ICouchDbInfoB
{
    private readonly IMyCouchFactory _factory;

    public CouchStoreBuilderT(IMyCouchFactory factory)
    {
        _factory = factory;
    }

    public IDocStoreT<TDoc> Build()
    {
        return new CouchDocStoreT<TDbInfo, TDoc, TID>(_factory);
    }

    public ICouchAdminStore<TDbInfo> BuildAdmin()
    {
        return new CouchAdminStoreT<TDbInfo>(_factory);
    }
}