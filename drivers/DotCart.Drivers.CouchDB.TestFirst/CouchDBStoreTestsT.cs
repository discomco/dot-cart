using DotCart.Abstractions.Schema;
using DotCart.Defaults.CouchDb;
using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.Drivers.CouchDB.TestFirst;

public abstract class CouchDBStoreTestsT<TDbInfo, TDoc, TID>
    : StoreTestsT<TDbInfo, TDoc, TID>
    where TID : IID
    where TDoc : IState
    where TDbInfo : ICouchDbInfoB
{
    protected CouchDBStoreTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }


    [Fact]
    public void ShouldResolveCouchBuilder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var builder = TestEnv.ResolveRequired<ICouchStoreBuilder<TDbInfo, TDoc, TID>>();
        // THEN
        Assert.NotNull(builder);
    }
}