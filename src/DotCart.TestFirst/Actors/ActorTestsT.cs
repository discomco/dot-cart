using DotCart.Abstractions.Actors;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class ActorTestsT<TSpoke, TActor> 
    : IoCTests
    where TActor : IActorT<TSpoke> where TSpoke : ISpokeT<TSpoke>
{
    protected TActor _actor;


    protected ActorTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


    [Fact]
    public void ShouldResolveExchange()
    {
        // GIVEN
        // WHEN
        // THEN
    }
    
    [Fact]
    public void ShouldResolveEmitter()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var actor = TestEnv.ResolveActor<TSpoke,TActor>();
        // THEN
        Assert.NotNull(actor);
    }
    
    [Fact]
    public void ShouldBeNamedActor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var name = NameAtt.Get<TActor>();
        // THEN
        Assert.NotEmpty(name);
    }

    [Fact]
    public async Task ShouldActivateActor()
    {
        //var ts = new CancellationTokenSource();
        // GIVEN
        var ts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
        Assert.NotNull(TestEnv);
        var ac = TestEnv.ResolveActor<TSpoke,TActor>();
        Assert.NotNull(ac);
        // WHEN
        await ac.Activate(ts.Token);
        // THEN
        Assert.True(ac.Status.HasFlag(ComponentStatus.Active));
    }

    
}