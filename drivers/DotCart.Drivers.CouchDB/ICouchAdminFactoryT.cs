using DotCart.Abstractions.Drivers;
using DotCart.Defaults.CouchDb;

namespace DotCart.Drivers.CouchDB;

public interface ICouchAdminFactoryT<TDbInfo>
    : IStoreFactoryB
    where TDbInfo : ICouchDbInfoB
{
    ICouchAdminStore<TDbInfo> BuildAdmin();
}