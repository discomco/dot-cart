using Castle.Core.Internal;
using DotCart.Contract;
using DotCart.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst;

public abstract class ContractTests<TID, THope, TFact, TPayload> : IoCTests
    where TID : IID
    where THope : IHope
    where TFact : IFact
    where TPayload : IPayload
{

    protected NewID<TID> NewID;

    protected ContractTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    [Fact]
    public void ShouldHaveHopeTopic()
    {
        // GIVEN
        // WHEN
        var topic = Topic.Get<THope>();
        // THEN
        Assert.False(topic.IsNullOrEmpty());
    }

    [Fact]
    public void ShouldHaveFactTopic()
    {
        // GIVEN
        // WHEN
        var topic = Topic.Get<TFact>();
        // THEN
        Assert.False(topic.IsNullOrEmpty());
    }

    [Fact]
    public abstract void ShouldCreateNewPayload();

    [Fact]
    public abstract void ShouldCreateFactFromBytes();

    [Fact]
    public abstract void ShouldCreateFactFromPayload();

    [Fact]
    public abstract void ShouldCreateHopeFromBytes();

    [Fact]
    public abstract void ShouldCreateHopeFromPayload();

    [Fact]
    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var ctor = Container.GetRequiredService<NewID<TID>>();
        // THEN
        Assert.NotNull(ctor);
    }

    [Fact]
    public void ShouldCreateID()
    {
        // GIVEN
        Assert.NotNull(NewID);
        // WHEN
        var ID = NewID();
        // THEN
        Assert.NotNull(ID);
    }

    [Fact]
    public void ShouldIDBeOfTypeTID()
    {
        // GIVEN
        Assert.NotNull(NewID);
        // WHEN
        var ID = NewID();
        // THEN
        Assert.IsType<TID>(ID);
    }
    
    

    protected override void Initialize()
    {
        NewID = Container.GetRequiredService<NewID<TID>>();
    }

}