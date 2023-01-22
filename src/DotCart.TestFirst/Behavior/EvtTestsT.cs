using System.Threading.Tasks;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Behavior;

public abstract class EvtTestsT<TID, TPayload, TMeta> : IoCTests
    where TID : IID
    where TPayload : IPayload
    where TMeta : IMetaB
{
    protected EvtCtorT<TPayload, TMeta> _newEvt;
    protected IDCtorT<TID> _newID;
    protected PayloadCtorT<TPayload> _newPayload;

    protected EvtTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    [Fact]
    public async Task ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        // THEN
        Assert.NotNull(_newID);
    }

    [Fact]
    public async Task ShouldResolveEvtCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newEvt = TestEnv.ResolveRequired<EvtCtorT<TPayload, TMeta>>();
        // THEN
        Assert.NotNull(_newEvt);
    }

    [Fact]
    public async Task ShouldResolvePayloadCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newPayload = TestEnv.ResolveRequired<PayloadCtorT<TPayload>>();
        // THEN
        Assert.NotNull(_newPayload);
    }

    [Fact]
    public async Task ShouldHaveTopicAttribute()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var expectedTopic = TopicAtt.Get(this);
        // WHEN
        var actualTopic = EvtTopicAtt.Get<TPayload>();
        // THEN
        Assert.Equal(expectedTopic, actualTopic);
    }
}