using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class ProjectionTestsT<
    TSpoke,
    TProjection,
    TState,
    TPayload, TMeta> : IoCTests
    where TProjection : IActorT<TSpoke>
    where TState : IState
    where TSpoke : ISpokeT<TSpoke>
    where TPayload : IPayload
    where TMeta : IEventMeta
{
    private Evt2Doc<TState, TPayload, TMeta> _evt2State;

    private IExchange _exchange;
    private IActorT<TSpoke>? _projection;

    public ProjectionTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


    // TODO : REACTIVATE THIS AFTER THE INTERVIEW
    [Fact]
    public void ShouldResolveProjection()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _projection = TestEnv.ResolveActor<TSpoke, TProjection>();
        // THEN 
        Assert.NotNull(_projection);
    }

    [Fact]
    public void ShouldResolveEvt2State()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _evt2State = TestEnv.ResolveRequired<Evt2Doc<TState, TPayload, TMeta>>();
        // THEN
        Assert.NotNull(_evt2State);
    }

    [Fact]
    public void ShouldResolveExchange()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _exchange = TestEnv.ResolveRequired<IExchange>();
        // THEN
        Assert.NotNull(_exchange);
    }

    [Fact]
    public async Task ShouldHaveNameAttribute()
    {
        // GIVEN 
        Assert.NotNull(TestEnv);
        // WHEN
        var name = NameAtt.Get<TProjection>();
        // THEN
        Assert.NotEmpty(name);
    }

    [Fact]
    public async Task ShouldPayloadHaveFactTopic()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var topic = FactTopicAtt.Get<TPayload>();
        // THEN
        Assert.NotEmpty(topic);
    }

    [Fact]
    public async Task ShouldHaveDbNameAttribute()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN 
        var dbName = DbNameAtt.Get<TProjection>();
        // THEN
        Assert.NotEmpty(dbName);
    }

    [Fact]
    public async Task ShouldSpokeHaveNameAttribute()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN 
        var spokeName = NameAtt.Get<TSpoke>();
        // THEN
        Assert.NotEmpty(spokeName);
    }

    [Fact]
    public void ShouldResolveReadModelStore()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var rms = TestEnv.ResolveRequired<IDocStore<TState>>();
        // THEN
        Assert.NotNull(rms);
    }
}