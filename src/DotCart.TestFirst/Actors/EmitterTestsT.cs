using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class EmitterTestsT<TSpoke, TEmitter, TFactPayload, TMeta, TID>
    : ActorTestsT<TSpoke, TEmitter>
    where TFactPayload : IPayload
    where TMeta : IMetaB
    where TSpoke : ISpokeT<TSpoke>
    where TEmitter : IActorT<TSpoke>
    where TID : IID
{
    protected Evt2Fact<TFactPayload, TMeta> _evt2Fact;

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
        _evt2Fact = TestEnv.ResolveRequired<Evt2Fact<TFactPayload, TMeta>>();
        // THEN
        Assert.NotNull(_evt2Fact);
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