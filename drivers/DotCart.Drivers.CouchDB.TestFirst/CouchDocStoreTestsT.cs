using DotCart.Abstractions.Schema;
using DotCart.Defaults.CouchDb;
using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.Drivers.CouchDB.TestFirst;

public abstract class CouchDocStoreTestsT<TDbInfo, TDoc, TID>
    : DocStoreTestsT<TDbInfo, TDoc, TID>
    where TID : IID
    where TDoc : IState
    where TDbInfo : ICouchDbInfoB
{
    protected CouchDocStoreTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
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