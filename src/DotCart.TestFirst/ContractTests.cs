using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst;

public abstract class ContractTests<TID, THope, TFact, TPayload> : IoCTests
    where TID : IID
    where THope : IHope
    where TFact : IFact
    where TPayload : IPayload
{
    protected NewID<TID> _newID;

    protected ContractTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldHaveHopeTopic()
    {
        // GIVEN
        // WHEN
        var topic = TopicAtt.Get<THope>();
        // THEN
        Assert.False(string.IsNullOrEmpty(topic));
    }

    [Fact]
    public void ShouldHaveFactTopic()
    {
        // GIVEN
        // WHEN
        var topic = TopicAtt.Get<TFact>();
        // THEN
        Assert.False(string.IsNullOrEmpty(topic));
    }

    [Fact]
    public abstract void ShouldCreateNewPayload();

    [Fact]
    public abstract void ShouldCreateNewFact();

    [Fact]
    public abstract void ShouldCreateNewHope();

    [Fact]
    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var ctor = TestEnv.ResolveRequired<NewID<TID>>();
        // THEN
        Assert.NotNull(ctor);
    }

    [Fact]
    public void ShouldCreateID()
    {
        // GIVEN
        Assert.NotNull(_newID);
        // WHEN
        var ID = _newID();
        // THEN
        Assert.NotNull(ID);
    }

    [Fact]
    public void ShouldIDBeOfTypeTID()
    {
        // GIVEN
        Assert.NotNull(_newID);
        // WHEN
        var ID = _newID();
        // THEN
        Assert.IsType<TID>(ID);
    }


    protected override void Initialize()
    {
        _newID = TestEnv.ResolveRequired<NewID<TID>>();
    }
}