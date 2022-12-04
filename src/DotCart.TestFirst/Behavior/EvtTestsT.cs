using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Behavior;

public abstract class EvtTestsT<TID, TEvt, TPayload> : IoCTests 
    where TID : IID 
    where TEvt : IEvt 
    where TPayload : IPayload
{
    protected IDCtorT<TID> _newID;
    protected EvtCtorT<TEvt,TID> _newEvt;
    protected PayloadCtorT<TPayload> _newPayload;

    public EvtTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
        _newEvt = TestEnv.ResolveRequired<EvtCtorT<TEvt, TID>>();
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
        var actualTopic = TopicAtt.Get<TEvt>();
        // THEN
        Assert.Equal(expectedTopic, actualTopic);
    }
  
    
    
}