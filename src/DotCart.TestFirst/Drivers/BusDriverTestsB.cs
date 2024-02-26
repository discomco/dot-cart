using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Drivers;

public abstract class BusDriverTestsB<TPayload, TMeta>
    : IoCTests
    where TMeta : IMetaB
    where TPayload : IPayload
{
    protected BusDriverTestsB(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldTPayloadHaveTopic()
    {
        // GIVEN
        var topic = FactTopicAtt.Get<TPayload>();
        // THEN
        Assert.NotEmpty(topic);
    }
}