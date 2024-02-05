using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class ListenerTestsT<TSpoke, TActor, TCmdPayload, TMeta, TFactPayload, TPipeInfo, TID>
    : ActorTestsT<TSpoke, TActor>
    where TFactPayload : IPayload
    where TSpoke : ISpokeT<TSpoke>
    where TActor : IActorT<TSpoke>
    where TMeta : IMetaB
    where TCmdPayload : IPayload
    where TID : IID
    where TPipeInfo : IPipeInfoB
{
    protected ListenerTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
    public void ShouldResolveListener()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var listener = TestEnv.ResolveRequired<IListenerT<TSpoke, TCmdPayload, TMeta, TFactPayload, TPipeInfo>>();
        // THEN
        Assert.NotNull(listener);
    }


    [Fact]
    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        // THEN
        var id = newID();
        Assert.NotNull(id);
    }

    [Fact]
    public void ShouldResolvePayloadCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var newPayload = TestEnv.ResolveRequired<PayloadCtorT<TFactPayload>>();
        // THEN
        var pl = newPayload();
        Assert.NotNull(pl);
    }

    [Fact]
    public void ShouldResolveMetaCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var newMeta = TestEnv.ResolveRequired<MetaCtorT<TMeta>>();
        // THEN
        Assert.NotNull(newMeta);
    }


    [Fact]
    public void ShouldResolveFactCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var newFact = TestEnv.ResolveRequired<FactCtorT<TFactPayload, TMeta>>();
        var newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        var newMeta = TestEnv.ResolveRequired<MetaCtorT<TMeta>>();
        var newPayload = TestEnv.ResolveRequired<PayloadCtorT<TFactPayload>>();
        // THEN
        var id = newID().Id();
        var fact = newFact(id, newPayload(), newMeta(id));
        Assert.NotNull(fact);
    }
}