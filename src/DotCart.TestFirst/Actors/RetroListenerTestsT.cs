using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class RetroListenerTestsT<TSpoke, TActor, TFactPayload, TMeta, TPipeInfo, TID>
    : ListenerTestsT<TSpoke, TActor, Dummy, TMeta, TFactPayload, TPipeInfo, TID>
    where TSpoke : ISpokeT<TSpoke>
    where TActor : IActorT<TSpoke>
    where TFactPayload : IPayload
    where TMeta : IMetaB
    where TPipeInfo : IPipeInfoB
    where TID : IID
{
    protected RetroListenerTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveEmitter()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var emitter = TestEnv.ResolveRequired<IEmitterT<TSpoke, TFactPayload, TMeta>>();
        // THEN
        Assert.NotNull(emitter);
    }

    [Fact]
    public void ShouldListenOnTopic()
    {
        // GIVEN
        // WHEN
        // THEN
    }
}