using DotCart.Abstractions.Drivers;
using DotCart.Core;
using DotCart.Defaults.CouchDb;
using Engine.Contract;

namespace Engine.Context;

[DbName(DbConstants.CouchListDbName)]
[Replicate(false)]
public interface ICouchListDbInfo
    : ICouchDbInfoB
{
}