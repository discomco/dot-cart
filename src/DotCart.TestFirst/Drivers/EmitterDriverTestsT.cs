using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Drivers;

public abstract class EmitterDriverTestsT<TEmitterDriver, TPayload, TMeta>
    : BusDriverTestsB<TPayload, TMeta>
    where TPayload : IPayload
    where TMeta : IMetaB
    where TEmitterDriver : IEmitterDriverB
{
    protected EmitterDriverTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveEmitterDriver()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        using var driver = TestEnv.ResolveRequired<TEmitterDriver>();
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