using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.Schema.Tests;

public abstract class IDTests<TID>: IoCTests where TID: ID
{
    protected NewID<TID> _newID;

    [Fact]
    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var newID = Container.GetRequiredService<NewID<TID>>();
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
    
    
    public IDTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    protected override void Initialize()
    {
        _newID = Container.GetRequiredService<NewID<TID>>();
    }


}