using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class EmitterTestsT<TSpoke, TEmitter, TPayload, TMeta> 
    : ActorTestsT<TSpoke, TEmitter>
    where TPayload : IPayload
    where TMeta : IEventMeta
    where TSpoke : ISpokeT<TSpoke>
    where TEmitter : IActorT<TSpoke>
{
    protected Evt2Fact<TPayload, TMeta> _evt2Fact;

    protected EmitterTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveEvt2Fact()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _evt2Fact = TestEnv.ResolveRequired<Evt2Fact<TPayload, TMeta>>();
        // THEN
        Assert.NotNull(_evt2Fact);
    }
 
    
    
}