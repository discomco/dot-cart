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
    TReadModelStore,
    TProjection,
    TState,
    TEvt> : IoCTests
    where TProjection : IActor<TSpoke>
    where TState : IState
    where TEvt : IEvt
    where TSpoke : ISpokeT<TSpoke>
    where TReadModelStore : IModelStore<TState>

{
    private Evt2State<TState, TEvt> _evt2State;

    private IExchange _exchange;
    private IActor<TSpoke>? _projection;

    public ProjectionTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


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
        _evt2State = TestEnv.ResolveRequired<Evt2State<TState, TEvt>>();
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
    public async Task ShouldEventHaveATopic()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var topic = TopicAtt.Get<TEvt>();
        // THEN
        Assert.NotEmpty(topic);
    }

    [Fact]
    public async void ShouldSpokeHaveNameAttribute()
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
        var rms = TestEnv.ResolveRequired<TReadModelStore>();
        // THEN
        Assert.NotNull(rms);
    }
}