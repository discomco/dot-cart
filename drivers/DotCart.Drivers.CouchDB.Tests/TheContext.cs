using DotCart.Core;
using DotCart.Defaults.CouchDb;
using DotCart.TestKit.Mocks;

namespace DotCart.Drivers.CouchDB.Tests;

public static class TheContext
{
    [DbName(TheConstants.CouchDocDbName)]
    [Replicate(false)]
    public interface ICouchDocDbInfo : ICouchDbInfoB
    {
    }

    [DbName(TheConstants.CouchListDbName)]
    [Replicate(false)]
    public interface ICouchListDbInfo : ICouchDbInfoB
    {
    }
}