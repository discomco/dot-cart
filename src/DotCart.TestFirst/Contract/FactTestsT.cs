using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Core;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Contract;

public abstract class FactTestsT<TID, TPayload, TMeta> : IoCTests
    where TID : IID
    where TPayload : IPayload
{
    protected FactCtorT<TPayload, TMeta> _newFact;
    protected IDCtorT<TID> _newID;
    private MetaCtorT<TMeta> _newMeta;
    protected PayloadCtorT<TPayload> _newPayload;

    protected FactTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
        _newFact = TestEnv.ResolveRequired<FactCtorT<TPayload, TMeta>>();
        Assert.NotNull(_newFact);
    }


    [Fact]
    public void ShouldDeserializeFact()
    {
        Assert.NotNull(_newID);
        Assert.NotNull(_newFact);
        var aggId = _newID().Id();
        Assert.NotEmpty(aggId);
        var fact = _newFact(aggId, _newPayload(), _newMeta(aggId));
        var serialized = fact.ToJson();
        var desFact = serialized.FromJson<FactT<TPayload, TMeta>>();
        Assert.Equal(fact, desFact);
    }

    [Fact]
    public void ShouldResolveMetaCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newMeta = TestEnv.ResolveRequired<MetaCtorT<TMeta>>();
        // THEN
        Assert.NotNull(_newMeta);
    }

    [Fact]
    protected void ShouldHaveFactTopic()
    {
        // GIVEN
        var expectedTopic = GetExpectedTopic();
        // WHEN
        var foundTopic = FactTopicAtt.Get<TPayload>();
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
        _newFact = TestEnv.ResolveRequired<FactCtorT<TPayload, TMeta>>();
        _newMeta = TestEnv.ResolveRequired<MetaCtorT<TMeta>>();
    }
}