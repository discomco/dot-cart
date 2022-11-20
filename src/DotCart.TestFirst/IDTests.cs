using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst;

public abstract class IDTests<TID> : IoCTests where TID : ID
{
    protected NewID<TID> _newID;


    public IDTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var newID = TestEnv.ResolveRequired<NewID<TID>>();
        // THEN
        Assert.NotNull(newID);
    }

    [Fact]
    public void ShouldCreateID()
    {
        // GIVEN
        Assert.NotNull(_newID);
        // WHEN
        var newID = _newID();
        // THEN
        Assert.NotNull(newID);
        Assert.Equal(GetIDPrefix(), newID.Prefix);
    }

    private string GetIDPrefix()
    {
        return IDPrefix.Get<TID>();
    }

    [Fact]
    public void ShouldCreateFromIdString()
    {
        // GIVEN
        Assert.NotNull(_newID);
        // WHEN
        var newID = _newID();
        var newerID = newID.Id().IDFromIdString();
        Assert.NotNull(newerID);
        Assert.Equal(newID.Id(), newerID.Id());
    }

    protected override void Initialize()
    {
        _newID = TestEnv.ResolveRequired<NewID<TID>>();
    }
}