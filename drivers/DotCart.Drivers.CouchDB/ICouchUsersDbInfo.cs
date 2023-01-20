using DotCart.Core;
using DotCart.Defaults.CouchDb;

namespace DotCart.Drivers.CouchDB;

[DbName(CouchConstants.UsersDbName)]
[Replicate(false)]
public interface ICouchUsersDbInfo : ICouchDbInfoB
{
}