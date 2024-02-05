using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.Drivers.NATS.TestFirst;

public abstract class NATSResponderDriverTestsT<TPayload>
    : IoCTests
    where TPayload : IPayload
{
    protected NATSResponderDriverTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }


    [Fact]
    public void PayloadShouldHaveHopeTopic()
    {
        // GIVEN
        // WHEN
        var hopeTopic = HopeTopicAtt.Get<TPayload>();
        // THEN
        Assert.NotNull(hopeTopic);
        Assert.NotEmpty(hopeTopic);
    }


    [Fact]
    public void ShouldResolveLogger()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var logger = TestEnv.ResolveRequired<ILogger>();
        // THEN
        Assert.NotNull(logger);
    }

    [Fact]
    public void ShouldResolveDriver()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var driver = TestEnv.ResolveRequired<INATSResponderDriverT<TPayload>>();
        // THEN
        Assert.NotNull(driver);
    }
}