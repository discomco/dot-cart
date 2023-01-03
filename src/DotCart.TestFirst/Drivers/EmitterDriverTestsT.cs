using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Drivers;

public abstract class EmitterDriverTestsT<TPayload, TMeta>
    : BusDriverTestsB<TPayload,TMeta>
    where TPayload : IPayload
    where TMeta : IEventMeta
{
    protected EmitterDriverTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveEmitterDriver()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        using var driver = TestEnv.ResolveRequired<IEmitterDriverT<TPayload,TMeta>>();
        // THEN
        Assert.NotNull(driver);
    }


    [Fact]
    public void ShouldResolveFact2Msg()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var fact2Msg = TestEnv.ResolveRequired<Fact2Msg<byte[], TPayload, TMeta>>();
        // THEN
        Assert.NotNull(fact2Msg);
    }
}