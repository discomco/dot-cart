using DotCart.Core;
using DotCart.Defaults.CouchDb;

namespace DotCart.Drivers.CouchDB;

[DbName(CouchConstants.ReplicatorDbName)]
[Replicate(false)]
public interface ICouchReplicationDbInfo : ICouchDbInfoB
{
}