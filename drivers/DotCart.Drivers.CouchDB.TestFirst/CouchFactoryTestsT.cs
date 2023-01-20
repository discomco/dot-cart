using DotCart.Core;
using DotCart.TestKit;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.Drivers.CouchDB.TestFirst;

public abstract class CouchFactoryTestsT<TDbInfo>
    : IoCTests
{
    public CouchFactoryTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }


    [Fact]
    public void ShouldResolveCouchFactory()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var factory = TestEnv.ResolveRequired<IMyCouchFactory>();
        // THEN
        Assert.NotNull(factory);
    }

    [Fact]
    public void ShouldCreateClient()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var fact = TestEnv.ResolveRequired<IMyCouchFactory>();
        // WHEN
        var clt = fact.Client(DbNameAtt.Get<TDbInfo>());
        // THEN
        Assert.NotNull(clt);
    }


    [Fact]
    public void ShouldCreateStore()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var fact = TestEnv.ResolveRequired<IMyCouchFactory>();
        // WHEN
        var store = fact.Store(DbNameAtt.Get<TDbInfo>());
        // THEN
        Assert.NotNull(store);
    }

    [Fact]
    public void ShouldCreateServer()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var fact = TestEnv.ResolveRequired<IMyCouchFactory>();
        // WHEN
        var server = fact.Server();
        // THEN
        Assert.NotNull(server);
    }

    [Fact]
    public async Task ShouldConnectToServer()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var fact = TestEnv.ResolveRequired<IMyCouchFactory>();
        var server = fact.Server();
        Assert.NotNull(server);
        // WHEN
        var resp = await server.Databases.HeadAsync(DbNameAtt.Get<TDbInfo>()).ConfigureAwait(false);
        // THEN
        Assert.NotNull(resp);
    }
}