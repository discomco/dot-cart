using DotCart.Abstractions.Drivers;
using DotCart.Defaults.CouchDb;

namespace DotCart.Drivers.CouchDB;

public interface ICouchAdminBuilderT<TDbInfo> 
    : IStoreBuilderB 
    where TDbInfo : ICouchDbInfoB
{
    ICouchAdminStore<TDbInfo> BuildAdmin();
}