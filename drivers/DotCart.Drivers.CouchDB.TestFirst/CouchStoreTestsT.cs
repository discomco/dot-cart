using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Defaults.CouchDb;
using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.Drivers.CouchDB.TestFirst;

public abstract class CouchStoreTestsT<TDbInfo, TDoc, TID>
    : DocStoreTestsT<TDbInfo, TDoc, TID>
    where TID : IID
    where TDoc : IState
    where TDbInfo : ICouchDbInfoB
{
    protected CouchStoreTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }


    // [Fact]
    // public void ShouldResolveCouchBuilder()
    // {
    //     // GIVEN
    //     Assert.NotNull(TestEnv);
    //     // WHEN
    //     var builder = TestEnv.ResolveRequired<IStoreFactoryT<TDbInfo, TDoc, TID>>();
    //     // THEN
    //     Assert.NotNull(builder);
    // }

    [Fact]
    public void ShouldCreateDocStore()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var builder = TestEnv.ResolveRequired<IStoreFactoryT<TDbInfo, TDoc, TID>>();
        Assert.NotNull(builder);
        var docStore = builder.Create();
        // THEN
        Assert.NotNull(docStore);
        Assert.IsType<CouchDocStoreT<TDbInfo, TDoc, TID>>(docStore);
    }


    [Fact]
    public void ShouldKnowReplicateAtt()
    {
        // GIVEN
        // WHEN
        var repl = ReplicateAtt.Get<TDbInfo>();
        // THEN
        Assert.True(true);
    }
}