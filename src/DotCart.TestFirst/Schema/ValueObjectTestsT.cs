using System.Reflection;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Schema;

public abstract class ValueObjectTestsT<TValueObject> : IoCTests
    where TValueObject : IValueObject
{
    private ValueObjectCtorT<TValueObject> _ctor;

    protected ValueObjectTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveValueObjectCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _ctor = TestEnv.ResolveRequired<ValueObjectCtorT<TValueObject>>();
        // THEN
        Assert.NotNull(_ctor);
    }

    [Fact]
    public void ShouldDeserializeValueObject()
    {
        // GIVEN
        var valueObject = CreateValueObject();
        var serialized = valueObject.ToJson();
        // WHEN
        var deserialized = serialized.FromJson<TValueObject>();
        // THEN
        Assert.Equal(valueObject, deserialized);
    }

    protected abstract TValueObject CreateValueObject();


    [Fact]
    public void ShouldBeSerializeValueObject()
    {
        // GIVEN
        var valueObject = CreateValueObject();
        var serialized = valueObject.ToJson();
        // WHEN
        var pubProps = typeof(TValueObject).GetProperties(BindingFlags.Public);
        foreach (var propertyInfo in pubProps) Assert.Contains(propertyInfo.Name, serialized);
    }
}