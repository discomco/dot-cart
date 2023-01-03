using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Drivers;

public abstract class BusDriverTestsB<TPayload, TMeta>
    : IoCTests
    where TMeta : IEventMeta
    where TPayload : IPayload
{
    private FactCtorT<TPayload,TMeta> _newFact;

    protected BusDriverTestsB(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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

    [Fact]
    public void ShouldResolveFactCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newFact = TestEnv.ResolveRequired<FactCtorT<TPayload,TMeta>>();
        // THEN
        Assert.NotNull(_newFact);
    }
}