using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Drivers;

public abstract class EmitterDriverTestsT<TIFact, TPayload>
    : BusDriverTestsB<TIFact, TPayload>
    where TIFact : IFactB
    where TPayload : IPayload
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
        using var driver = TestEnv.ResolveRequired<IEmitterDriverT<TPayload>>();
        // THEN
        Assert.NotNull(driver);
    }


    [Fact]
    public void ShouldResolveFact2Msg()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var fact2Msg = TestEnv.ResolveRequired<Fact2Msg<byte[], TPayload>>();
        // THEN
        Assert.NotNull(fact2Msg);
    }
}