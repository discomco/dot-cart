using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Drivers;

public abstract class BusDriverTestsB<TIFact, TPayload, TDriverMsg> 
    : IoCTests
    where TIFact : IFactB
    where TPayload : IPayload
    where TDriverMsg : class
{
    private FactCtorT<TPayload> _newFact;

    protected BusDriverTestsB(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldTIFactHaveTopic()
    {
        // GIVEN
        var topic = TopicAtt.Get<TIFact>();
        // THEN
        Assert.NotEmpty(topic);
    }

    [Fact]
    public void ShouldResolveFactCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newFact = TestEnv.ResolveRequired<FactCtorT<TPayload>>();
        // THEN
        Assert.NotNull(_newFact);
    }

}