using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Contract;

public abstract class HopeTestsT<TID, TPayload>
    : IoCTests
    where TID : IID
    where TPayload : IPayload

{
    protected HopeCtorT<TPayload> _newHope;
    protected IDCtorT<TID> _newID;
    protected PayloadCtorT<TPayload> _newPayload;

    protected HopeTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
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
    public void ShouldResolveHopeCtor()
    {
        Assert.NotNull(TestEnv);
        _newHope = TestEnv.ResolveRequired<HopeCtorT<TPayload>>();
        Assert.NotNull(_newHope);
    }


    [Fact]
    public void ShouldDeserializeHope()
    {
        Assert.NotNull(_newID);
        Assert.NotNull(_newHope);
        var aggId = _newID().Id();
        Assert.NotEmpty(aggId);
        var hope = _newHope(aggId, _newPayload());
        var serialized = hope.ToJson();
        var desHope = serialized.FromJson<HopeT<TPayload>>();
        Assert.Equal(hope, desHope);
    }

    [Fact]
    protected void ShouldHaveTopic()
    {
        // GIVEN
        var expectedTopic = GetExpectedTopic();
        // WHEN
        var foundTopic = HopeTopicAtt.Get<TPayload>();
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
        _newHope = TestEnv.ResolveRequired<HopeCtorT<TPayload>>();
    }
}