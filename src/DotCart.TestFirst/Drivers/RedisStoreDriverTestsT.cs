using DotCart.Context.Effects.Drivers;
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


    public RedisStoreDriverTestsT(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }


    [Fact]
    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        _newID = Container.GetRequiredService<NewID<TID>>();
        // THEN
        Assert.NotNull(_newID);
    }

    [Fact]
    public void ShouldResolveStateCtor()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        _newState = Container.GetRequiredService<NewState<TState>>();
        // THEN
        Assert.NotNull(_newState);
    }


    [Fact]
    public void ShouldResolveRedisDB()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        _redisDB = Container.GetRequiredService<IRedisDb>();
        // THEN
        Assert.NotNull(_redisDB);
    }

    [Fact]
    public void ShouldResolveConnectionMultiplexer()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        _connection = Container.GetRequiredService<IConnectionMultiplexer>();
        // THEN
        Assert.NotNull(_connection);
    }

    [Fact]
    public void ShouldResolveRedisStoreDriver()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        _redisStore = Container.GetRequiredService<IModelStore<TState>>();
        // THEN
        Assert.NotNull(_redisStore);
    }


    [Fact]
    public async Task ShouldSetDocToRedisStore()
    {
        // GIVEN
        Assert.NotNull(Container);
        _redisStore = Container.GetRequiredService<IModelStore<TState>>();
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
        Assert.NotNull(Container);
        _redisStore = Container.GetRequiredService<IModelStore<TState>>();
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
        Assert.NotNull(Container);
        _redisStore = Container.GetRequiredService<IModelStore<TState>>();
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
        Assert.NotNull(Container);
        _redisStore = Container.GetRequiredService<IModelStore<TState>>();
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
        _redisStore = Container.GetRequiredService<IModelStore<TState>>();
        _connection = Container.GetRequiredService<IConnectionMultiplexer>();
        _redisDB = Container.GetRequiredService<IRedisDb>();
        _newID = Container.GetRequiredService<NewID<TID>>();
        _newState = Container.GetRequiredService<NewState<TState>>();
    }
}