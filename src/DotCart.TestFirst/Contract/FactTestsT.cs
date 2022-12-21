using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Contract;

public abstract class FactTestsT<TID, TFact, TPayload> : IoCTests
    where TID : IID
    where TFact : IFactT<TPayload>
    where TPayload : IPayload
{
    protected FactCtorT<TFact, TPayload> _newFact;
    protected IDCtorT<TID> _newID;
    protected PayloadCtorT<TPayload> _newPayload;

    public FactTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


    [Fact]
    public void ShouldResolveIDCtor()
    {
        Assert.NotNull(TestEnv);
        _newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        Assert.NotNull(_newID);
    }

    [Fact]
    public void ShouldResolvePayloadCtor()
    {
        Assert.NotNull(TestEnv);
        _newPayload = TestEnv.ResolveRequired<PayloadCtorT<TPayload>>();
        Assert.NotNull(_newPayload);
    }

    [Fact]
    public void ShouldResolveFactCtor()
    {
        Assert.NotNull(TestEnv);
        _newFact = TestEnv.ResolveRequired<FactCtorT<TFact, TPayload>>();
        Assert.NotNull(_newFact);
    }


    [Fact]
    public void ShouldDeserializeFact()
    {
        Assert.NotNull(_newID);
        Assert.NotNull(_newFact);
        var aggId = _newID().Id();
        Assert.NotEmpty(aggId);
        var fact = _newFact(aggId, _newPayload());
        var serialized = fact.ToJson();
        var desFact = serialized.FromJson<TFact>();
        Assert.Equal(fact, desFact);
    }

    [Fact]
    protected void ShouldHaveTopic()
    {
        // GIVEN
        var expectedTopic = GetExpectedTopic();
        // WHEN
        var foundTopic = TopicAtt.Get<TFact>();
        // THEN
        Assert.Equal(expectedTopic, foundTopic);
    }

    private string GetExpectedTopic()
    {
        return TopicAtt.Get(this);
    }


    protected override void Initialize()
    {
        _newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        _newPayload = TestEnv.ResolveRequired<PayloadCtorT<TPayload>>();
        _newFact = TestEnv.ResolveRequired<FactCtorT<TFact, TPayload>>();
    }
}