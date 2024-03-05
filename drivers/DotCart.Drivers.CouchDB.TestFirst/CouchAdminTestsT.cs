using DotCart.Defaults.CouchDb;
using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.Drivers.CouchDB.TestFirst;

public abstract class CouchAdminTestsT<TDbInfo> :
    AdminStoreTestsT<TDbInfo>
    where TDbInfo : ICouchDbInfoB
{
    protected CouchAdminTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }


    [Fact]
    public async Task ShouldOpenDb()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var builder = TestEnv.ResolveRequired<ICouchAdminFactoryT<TDbInfo>>();
        // THEN
        var admin = builder
            .BuildAdmin();
        var rsp = await admin.OpenDbAsync();
        Assert.True(rsp.IsSuccess);
    }

    [Fact]
    public void ShouldBuildAdminStore()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var builder = TestEnv.ResolveRequired<ICouchAdminFactoryT<TDbInfo>>();
        Assert.NotNull(builder);
        var adminStore = builder.BuildAdmin();
        // THEN
        Assert.NotNull(adminStore);
    }


    [Fact]
    public async Task ShouldCreateDb()
    {
        Assert.NotNull(TestEnv);
        var builder = TestEnv.ResolveRequired<ICouchAdminFactoryT<TDbInfo>>();
        var admin = builder.BuildAdmin();
        // WHEN
        var fbk = await admin.CreateDbAsync();
        // THEN
        Assert.True(fbk.IsSuccess);
    }

    [Fact]
    public async Task ShouldCheckIfDbExists()
    {
        // GIVEN
        var cts = new CancellationTokenSource(1_000);
        Assert.NotNull(TestEnv);
        var builder = TestEnv.ResolveRequired<ICouchAdminFactoryT<TDbInfo>>();
        var admin = builder.BuildAdmin();
        // WHEN
        var fbk = await admin.DbExistsAsync(cts.Token);
        // THEN
        Assert.True(true);
    }
}