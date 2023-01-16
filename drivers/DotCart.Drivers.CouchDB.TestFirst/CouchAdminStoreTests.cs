using DotCart.Defaults.CouchDb;
using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.CouchDB.TestFirst;

public abstract class CouchAdminStoreTests<TDbInfo> : 
    AdminStoreTestsT<TDbInfo> 
    where TDbInfo : ICouchDbInfoB
{
    protected CouchAdminStoreTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


}