using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
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
        var builder = TestEnv.ResolveRequired<IStoreBuilderT<TDbInfo, TDoc, TID>>();
        // THEN
        Assert.NotNull(builder);
    }

    [Fact]
    public void ShouldBuildDocStore()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var builder = TestEnv.ResolveRequired<IStoreBuilderT<TDbInfo, TDoc, TID>>();
        Assert.NotNull(builder);
        var docStore = builder.Build();
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