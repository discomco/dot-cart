using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.Drivers.NATS.TestFirst;

public abstract class NATSListenerDriverTestsT<TPayload, TMeta>
    : IoCTests
    where TPayload : IPayload
    where TMeta : IMetaB
{
    protected NATSListenerDriverTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
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
    public void PayloadShouldHaveFactTopic()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var factTopic = FactTopicAtt.Get<TPayload>();
        // THEN
        Assert.NotNull(factTopic);
        Assert.NotEmpty(factTopic);
    }

    [Fact]
    public void ShouldResolveDriver()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var driver = TestEnv.ResolveRequired<INATSListenerDriverT<TPayload>>();
        // THEN
        Assert.NotNull(driver);
    }
}