using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Drivers;

public abstract class ListenerDriverTestsT<TListenerDriver, TPayload, TMeta>
    : BusDriverTestsB<TPayload, TMeta>
    where TPayload : IPayload
    where TMeta : IMetaB
{
    protected ListenerDriverTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveListenerDriver()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var driver = TestEnv.ResolveRequired<TListenerDriver>();
        // THEN
        Assert.NotNull(driver);
    }


    [Fact]
    public void ShouldResolveMsg2Fact()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var msg2Fact = TestEnv.ResolveRequired<Msg2Fact<TPayload, TMeta, byte[]>>();
        // THEN
        Assert.NotNull(msg2Fact);
    }
}