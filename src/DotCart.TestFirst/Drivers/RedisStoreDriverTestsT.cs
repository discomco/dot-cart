using DotCart.Context.Abstractions.Drivers;
using DotCart.Contract.Schemas;
using DotCart.Drivers.Redis;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Drivers;

public abstract class RedisStoreDriverTestsT<TID, TState> : IoCTests where TState : IState where TID : IID
{
    protected IConnectionMultiplexer _connection;
    protected NewID<TID> _newID;
    protected NewState<TState> _newState;
    protected IRedisDb _redisDB;
    protected IModelStore<TState> _redisStore;


    public RedisStoreDriverTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


    [Fact]
    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newID = TestEnv.GetRequiredService<NewID<TID>>();
        // THEN
        Assert.NotNull(_newID);
    }

    [Fact]
    public void ShouldResolveStateCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newState = TestEnv.GetRequiredService<NewState<TState>>();
        // THEN
        Assert.NotNull(_newState);
    }


    [Fact]
    public void ShouldResolveRedisDB()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _redisDB = TestEnv.GetRequiredService<IRedisDb>();
        // THEN
        Assert.NotNull(_redisDB);
    }

    [Fact]
    public void ShouldResolveConnectionMultiplexer()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _connection = TestEnv.GetRequiredService<IConnectionMultiplexer>();
        // THEN
        Assert.NotNull(_connection);
    }

    [Fact]
    public void ShouldResolveRedisStoreDriver()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _redisStore = TestEnv.GetRequiredService<IModelStore<TState>>();
        // THEN
        Assert.NotNull(_redisStore);
    }


    [Fact]
    public async Task ShouldSetDocToRedisStore()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        _redisStore = TestEnv.GetRequiredService<IModelStore<TState>>();
        Assert.NotNull(_redisStore);
        // WHEN
        var ID = _newID();
        var doc = _newState();
        var res = await _redisStore.SetAsync(ID.Id(), doc);
        // THEN
        Assert.NotNull(res);
        Assert.Equal(res, doc);
    }

    [Fact]
    public async Task ShouldDeleteDocFromRedisStore()
    {
        Assert.NotNull(TestEnv);
        _redisStore = TestEnv.GetRequiredService<IModelStore<TState>>();
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
        _redisStore = TestEnv.GetRequiredService<IModelStore<TState>>();
        Assert.NotNull(_redisStore);
        // WHEN
        var ID = _newID();
        var doc = _newState();
        var res = await _redisStore.SetAsync(ID.Id(), doc);
        Assert.NotNull(res);
        Assert.Equal(res, doc);
        // THEN
        var exists = await _redisStore.Exists(ID.Id());
        Assert.True(exists);
    }

    [Fact]
    public async Task ShouldGetDocFromRedisStore()
    {
        Assert.NotNull(TestEnv);
        _redisStore = TestEnv.GetRequiredService<IModelStore<TState>>();
        Assert.NotNull(_redisStore);
        // WHEN
        var ID = _newID();
        var doc = _newState();
        var res = await _redisStore.SetAsync(ID.Id(), doc);
        Assert.NotNull(res);
        Assert.Equal(res, doc);
        // THEN
        var getDoc = await _redisStore.GetByIdAsync(ID.Id());
        Assert.NotNull(getDoc);
        Assert.Equal(getDoc, doc);
    }


    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddSingletonRedisDb<TState>();
    }

    protected override void Initialize()
    {
        _redisStore = TestEnv.GetRequiredService<IModelStore<TState>>();
        _connection = TestEnv.GetRequiredService<IConnectionMultiplexer>();
        _redisDB = TestEnv.GetRequiredService<IRedisDb>();
        _newID = TestEnv.GetRequiredService<NewID<TID>>();
        _newState = TestEnv.GetRequiredService<NewState<TState>>();
    }
}