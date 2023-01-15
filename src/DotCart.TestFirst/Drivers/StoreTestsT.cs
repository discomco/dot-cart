using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Drivers;

public abstract class StoreTestsT<TDbInfo, TDoc, TID>
    : IoCTests
    where TDoc : IState
    where TID : IID
    where TDbInfo : IDbInfoB
{
    protected IDCtorT<TID> _newID;
    protected StateCtorT<TDoc> _newState;

    protected StoreTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


    [Fact]
    public void ShouldResolveStoreBuilder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var builder = TestEnv.ResolveRequired<IStoreBuilderT<TDbInfo, TDoc, TID>>();
        // THEN
        Assert.NotNull(builder);
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
    public async Task ShouldSetAsync()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        var newDoc = TestEnv.ResolveRequired<StateCtorT<TDoc>>();
        var builder = TestEnv.ResolveRequired<IStoreBuilderT<TDbInfo, TDoc, TID>>();
        Assert.NotNull(builder);
        var store = builder.Build();
        // THEN
        var ID = newID();
        var doc = newDoc();
        try
        {
            var res = await store.SetAsync(ID.Id(), doc);
            var exists = await store.Exists(ID.Id());
            Assert.True(exists);
            Assert.NotNull(res);
        }
        catch (Exception e)
        {
            Assert.Fail(AppErrors.Error(e.InnerAndOuter()));
        }
    }

    [Fact]
    public async Task ShouldDeleteAsync()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        var newDoc = TestEnv.ResolveRequired<StateCtorT<TDoc>>();
        var builder = TestEnv.ResolveRequired<IStoreBuilderT<TDbInfo, TDoc, TID>>();
        // THEN
        var store = builder.Build();
        var ID = newID();
        var doc = newDoc();
        var id = ID.Id();
        try
        {
            var res = await store.SetAsync(ID.Id(), doc);
            Assert.NotNull(res);
            var del = await store.DeleteAsync(ID.Id());
            Assert.NotNull(del);
        }
        catch (Exception e)
        {
            Assert.Fail(AppErrors.Error(e.InnerAndOuter()));
        }
    }

    [Fact]
    public async Task ShouldCheckExists()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        var newDoc = TestEnv.ResolveRequired<StateCtorT<TDoc>>();
        var builder = TestEnv.ResolveRequired<IStoreBuilderT<TDbInfo, TDoc, TID>>();
        Assert.NotNull(builder);
        var store = builder.Build();
        // THEN
        var ID = newID();
        var doc = newDoc();
        try
        {
            var res = await store.SetAsync(ID.Id(), doc);
            var exists = await store.Exists(ID.Id());
            Assert.True(exists);
            Assert.NotNull(res);
            var del = await store.DeleteAsync(ID.Id());
            exists = await store.Exists(ID.Id());
            Assert.False(exists);
        }
        catch (Exception e)
        {
            Assert.Fail(AppErrors.Error(e.InnerAndOuter()));
        }
    }

    [Fact]
    public async Task ShouldGetByIdAsync()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        var newDoc = TestEnv.ResolveRequired<StateCtorT<TDoc>>();
        var builder = TestEnv.ResolveRequired<IStoreBuilderT<TDbInfo, TDoc, TID>>();
        Assert.NotNull(builder);
        var store = builder.Build();
        // THEN
        var ID = newID();
        var doc = newDoc();
        try
        {
            var res = await store.SetAsync(ID.Id(), doc);
            var exists = await store.Exists(ID.Id());
            Assert.True(exists);
            Assert.NotNull(res);
            var stored = await store.GetByIdAsync(ID.Id());
            Assert.NotNull(stored);
            var del = await store.DeleteAsync(ID.Id());
            exists = await store.Exists(ID.Id());
            Assert.False(exists);
        }
        catch (Exception e)
        {
            Assert.Fail(AppErrors.Error(e.InnerAndOuter()));
        }
    }

    [Fact]
    public async Task ShouldCheckHasData()
    {
        Assert.NotNull(TestEnv);
        // WHEN
        var newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        var newDoc = TestEnv.ResolveRequired<StateCtorT<TDoc>>();
        var builder = TestEnv.ResolveRequired<IStoreBuilderT<TDbInfo, TDoc, TID>>();
        Assert.NotNull(builder);
        var store = builder.Build();
        // THEN
        var ID = newID();
        var doc = newDoc();
        try
        {
            var res = await store.SetAsync(ID.Id(), doc);

            var exists = await store.Exists(ID.Id());
            Assert.True(exists);
            Assert.NotNull(res);

            var hasData = await store.HasData();
            Assert.True(hasData);

            var del = await store.DeleteAsync(ID.Id());
            exists = await store.Exists(ID.Id());
            Assert.False(exists);
        }
        catch (Exception e)
        {
            Assert.Fail(AppErrors.Error(e.InnerAndOuter()));
        }
    }
}