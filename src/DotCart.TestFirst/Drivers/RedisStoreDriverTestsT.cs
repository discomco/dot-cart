using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Drivers.Redis;
using DotCart.TestKit;
using StackExchange.Redis;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Drivers;

public abstract class RedisStoreDriverTestsT<TID, TDoc> : IoCTests
    where TDoc : IState
    where TID : IID
{
    protected IConnectionMultiplexer _connection;
    protected IDCtorT<TID> _newID;
    protected StateCtorT<TDoc> _newState;
    protected IRedisDbT<TDoc> _redisDB;
    protected IDocStore<TDoc> _redisStore;


    public RedisStoreDriverTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


    [Fact]
    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        // THEN
        Assert.NotNull(_newID);
    }

    [Fact]
    public void ShouldResolveDocCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newState = TestEnv.ResolveRequired<StateCtorT<TDoc>>();
        // THEN
        Assert.NotNull(_newState);
    }


    [Fact]
    public void ShouldResolveRedisDB()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _redisDB = TestEnv.ResolveRequired<IRedisDbT<TDoc>>();
        // THEN
        Assert.NotNull(_redisDB);
    }

    [Fact]
    public void ShouldResolveConnectionMultiplexer()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _connection = TestEnv.ResolveRequired<IConnectionMultiplexer>();
        // THEN
        Assert.NotNull(_connection);
    }

    [Fact]
    public void ShouldResolveRedisStoreDriver()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _redisStore = TestEnv.ResolveRequired<IDocStore<TDoc>>();
        // THEN
        Assert.NotNull(_redisStore);
    }


    [Fact]
    public async Task ShouldSetDocToRedisStore()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        _redisStore = TestEnv.ResolveRequired<IDocStore<TDoc>>();
        Assert.NotNull(_redisStore);
        // WHEN
        var ID = _newID();
        var doc = _newState();
        var res = await _redisStore.SetAsync(ID.Id(), doc).ConfigureAwait(false);
        // THEN
        Assert.NotNull(res);
        Assert.Equal(res, doc);
        await _redisStore.DeleteAsync(ID.Id()).ConfigureAwait(false);
    }

    [Fact]
    public async Task ShouldDeleteDocFromRedisStore()
    {
        Assert.NotNull(TestEnv);
        _redisStore = TestEnv.ResolveRequired<IDocStore<TDoc>>();
        Assert.NotNull(_redisStore);
        // WHEN
        var ID = _newID();
        var doc = _newState();
        var res = await _redisStore.SetAsync(ID.Id(), doc);
        Assert.NotNull(res);
        Assert.Equal(res, doc);
        // THEN
        var delDoc = await _redisStore.DeleteAsync(ID.Id());
        Assert.Equal(res, delDoc);
    }


    [Fact]
    public async Task ShouldCheckIfDocExists()
    {
        Assert.NotNull(TestEnv);
        _redisStore = TestEnv.ResolveRequired<IDocStore<TDoc>>();
        Assert.NotNull(_redisStore);
        // WHEN
        var ID = _newID();
        var doc = _newState();
        var res = await _redisStore.SetAsync(ID.Id(), doc);
        Assert.NotNull(res);
        Assert.Equal(res, doc);
        // THEN
        var exists = await _redisStore.Exists(ID.Id()).ConfigureAwait(false);
        Assert.True(exists);
        await _redisStore.DeleteAsync(ID.Id()).ConfigureAwait(false);
    }

    [Fact]
    public async Task ShouldGetDocFromRedisStore()
    {
        Assert.NotNull(TestEnv);
        _redisStore = TestEnv.ResolveRequired<IDocStore<TDoc>>();
        Assert.NotNull(_redisStore);
        // WHEN
        var ID = _newID();
        var doc = _newState();
        var res = await _redisStore.SetAsync(ID.Id(), doc);
        Assert.NotNull(res);
        Assert.Equal(res, doc);
        // THEN
        var getDoc = await _redisStore.GetByIdAsync(ID.Id()).ConfigureAwait(false);
        Assert.NotNull(getDoc);
        Assert.Equal(getDoc, doc);
        await _redisStore.DeleteAsync(ID.Id()).ConfigureAwait(false);
    }


    protected override void Initialize()
    {
        _redisStore = TestEnv.ResolveRequired<IDocStore<TDoc>>();
        _connection = TestEnv.ResolveRequired<IConnectionMultiplexer>();
        _redisDB = TestEnv.ResolveRequired<IRedisDbT<TDoc>>();
        _newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        _newState = TestEnv.ResolveRequired<StateCtorT<TDoc>>();
    }
}