using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class EmitterTestT<TEmitter, TEvt, TFact> : IoCTests
    where TEmitter : IEmitterB
    where TEvt : IEvtB
    where TFact : IFactB
{
    protected TEmitter _emitter;
    protected Evt2Fact<TFact, TEvt> _evt2Fact;

    protected EmitterTestT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveEmitter()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _emitter = TestEnv.ResolveRequired<TEmitter>();
        // THEN
        Assert.NotNull(_emitter);
    }

    [Fact]
    public void ShouldResolveEvt2Fact()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _evt2Fact = TestEnv.ResolveRequired<Evt2Fact<TFact, TEvt>>();
        // THEN
        Assert.NotNull(_evt2Fact);
    }
}