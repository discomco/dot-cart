using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Schema;

public abstract class IDTestsT<TID>
    : IoCTests
    where TID : IID
{
    public IDTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    [Fact]
    public Task ShouldTestHaveIDPrefix()
    {
        // GIVEN
        Assert.IsAssignableFrom<IDTestsT<TID>>(this);
        // WHEN
        var idPrefix = IDPrefixAtt.Get(this);
        // THEN
        Assert.NotEmpty(idPrefix);
        return Task.CompletedTask;
    }


    [Fact]
    public Task ShouldHaveIDPrefix()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        var ID = newID();
        Assert.IsAssignableFrom<TID>(ID);

        var expected = IDPrefixAtt.Get(this);
        // WHEN
        var actual = IDPrefixAtt.Get<TID>();
        // THEN
        Assert.Equal(expected, actual);
        return Task.CompletedTask;
    }


    [Fact]
    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        // THEN
        Assert.NotNull(newID);
    }

    [Fact]
    public void ShouldCreateID()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var _newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        // WHEN
        var newID = _newID();
        // THEN
        Assert.NotNull(newID);
        Assert.Equal(GetIDPrefix(), IDPrefixAtt.Get(newID));
    }

    private string GetIDPrefix()
    {
        return IDPrefixAtt.Get<TID>();
    }

    [Fact]
    public void ShouldCreateFromIdString()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var _newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        // WHEN
        var newID = _newID();
        var newerID = newID.Id().IDFromIdString();
        Assert.NotNull(newerID);
        Assert.Equal(newID.Id(), newerID.Id());
    }
}