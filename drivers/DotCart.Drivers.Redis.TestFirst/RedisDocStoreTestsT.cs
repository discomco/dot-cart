using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Defaults.Redis;
using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.Drivers.Redis.TestFirst;

public abstract class RedisDocStoreTestsT<TDbInfo, TDoc, TID>
    : DocStoreTestsT<TDbInfo, TDoc, TID>
    where TDoc : IState
    where TID : IID
    where TDbInfo : IRedisDbInfoB
{
    private IRedisStoreBuilder<TDbInfo, TDoc, TID> _builder;
    protected IConnectionMultiplexer _connection;
    protected IRedisConnectionFactory<TDbInfo, TDoc> _connFact;
    protected IDocStoreT<TDoc> RedisStoreT;


    public RedisDocStoreTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddDotRedis<TDbInfo, TDoc, TID>();
    }

    protected override void Initialize()
    {
        Assert.NotNull(TestEnv);
        _connFact = TestEnv.ResolveRequired<IRedisConnectionFactory<TDbInfo, TDoc>>();
        Assert.NotNull(_connFact);
        _builder = TestEnv.ResolveRequired<IRedisStoreBuilder<TDbInfo, TDoc, TID>>();
        Assert.NotNull(_builder);
        RedisStoreT = _builder.Build();
        Assert.NotNull(RedisStoreT);
        _newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        Assert.NotNull(_newID);
        _newState = TestEnv.ResolveRequired<StateCtorT<TDoc>>();
    }


    [Fact]
    public void ShouldResolveRedisConnectionFactory()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _connFact = TestEnv.ResolveRequired<IRedisConnectionFactory<TDbInfo, TDoc>>();
        // THEN
        Assert.NotNull(_connFact);
    }


    [Fact]
    public void ShouldConnect()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _connFact = TestEnv.ResolveRequired<IRedisConnectionFactory<TDbInfo, TDoc>>();
        _connection = _connFact.Connect();
        // THEN
        Assert.NotNull(_connection);
    }


    [Fact]
    public async Task ShouldSetDocToRedisStore()
    {
        // GIVEN
        Assert.NotNull(RedisStoreT);
        Assert.NotNull(_newState);
        Assert.NotNull(_newID);
        // WHEN
        var ID = _newID();
        var doc = _newState();
        var res = await RedisStoreT.SetAsync(ID.Id(), doc).ConfigureAwait(false);
        // THEN
        Assert.NotNull(res);
        Assert.Equal(res, doc);
        await RedisStoreT.DeleteAsync(ID.Id()).ConfigureAwait(false);
    }

    [Fact]
    public async Task ShouldDeleteDocFromRedisStore()
    {
        // GIVEN
        // GIVEN
        Assert.NotNull(RedisStoreT);
        Assert.NotNull(_newState);
        Assert.NotNull(_newID);
        // WHEN
        var ID = _newID();
        var doc = _newState();
        var res = await RedisStoreT.SetAsync(ID.Id(), doc);
        Assert.NotNull(res);
        Assert.Equal(res, doc);
        // THEN
        var delDoc = await RedisStoreT.DeleteAsync(ID.Id());
        Assert.Equal(res, delDoc);
    }


    [Fact]
    public async Task ShouldCheckIfDocExists()
    {
        // GIVEN
        Assert.NotNull(RedisStoreT);
        Assert.NotNull(_newState);
        Assert.NotNull(_newID);
        // WHEN
        var ID = _newID();
        var doc = _newState();
        var res = await RedisStoreT.SetAsync(ID.Id(), doc);
        Assert.NotNull(res);
        Assert.Equal(res, doc);
        // THEN
        var exists = await RedisStoreT.Exists(ID.Id()).ConfigureAwait(false);
        Assert.True(exists);
        await RedisStoreT.DeleteAsync(ID.Id()).ConfigureAwait(false);
    }

    [Fact]
    public async Task ShouldGetDocFromRedisStore()
    {
        // GIVEN
        Assert.NotNull(RedisStoreT);
        Assert.NotNull(_newState);
        Assert.NotNull(_newID);
        // WHEN
        var ID = _newID();
        var doc = _newState();
        var res = await RedisStoreT.SetAsync(ID.Id(), doc);
        Assert.NotNull(res);
        Assert.Equal(res, doc);
        // THEN
        var getDoc = await RedisStoreT.GetByIdAsync(ID.Id()).ConfigureAwait(false);
        Assert.NotNull(getDoc);
        Assert.Equal(getDoc, doc);
        await RedisStoreT.DeleteAsync(ID.Id()).ConfigureAwait(false);
    }
}