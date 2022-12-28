using System.Reflection;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Schema;

public abstract class EntityTestsT<TID, TEntity> : IoCTests
    where TID : IID
    where TEntity : IEntityT<TID>
{
    protected IDCtorT<TID> _idCtor;

    protected EntityTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public async Task ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _idCtor = TestEnv.ResolveRequired<IDCtorT<TID>>();
        // THEN
        Assert.NotNull(_idCtor);
    }


    [Fact]
    public async Task ShouldBeSerializableEntity()
    {
        // GIVEN
        _idCtor = TestEnv.ResolveRequired<IDCtorT<TID>>();
        Assert.NotNull(_idCtor);
        var entity = CreateEntity(_idCtor);
        // WHEN
        var json = entity.ToJson();
        // THEN 
        Assert.NotNull(json);
        var props = typeof(TEntity).GetProperties(BindingFlags.Public);
        foreach (var info in props) Assert.Contains(info.Name, json);
    }


    [Fact]
    public async Task ShouldBeDeserializableEntity()
    {
        // GIVEN
        _idCtor = TestEnv.ResolveRequired<IDCtorT<TID>>();
        Assert.NotNull(_idCtor);
        var entity = CreateEntity(_idCtor);
        // AND
        var json = entity.ToJson();
        // WHEN
        var deserialized = json.FromJson<TEntity>();
        // THEN
        Assert.Equivalent(entity, deserialized);
    }

    protected abstract TEntity CreateEntity(IDCtorT<TID> idCtor);
}