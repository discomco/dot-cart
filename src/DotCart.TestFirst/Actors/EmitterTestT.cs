using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class EmitterTestT<TEmitter, TPayload, TMeta> : IoCTests
    where TEmitter : IEmitterB
    where TPayload : IPayload
    where TMeta : IEventMeta
{
    protected TEmitter _emitter;
    protected Evt2Fact<TPayload, TMeta> _evt2Fact;

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
        _evt2Fact = TestEnv.ResolveRequired<Evt2Fact<TPayload, TMeta>>();
        // THEN
        Assert.NotNull(_evt2Fact);
    }
}