using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Drivers;

public abstract class AsyncInfraDriverTestsT<TIFact, TPayload, TMsg> : IoCTests
    where TIFact : IFactB
    where TPayload : IPayload
    where TMsg : class
{
    private FactCtorT<TPayload> _newFact;

    protected AsyncInfraDriverTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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


    [Fact]
    public void ShouldResolveEmitterDriver()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        using var driver = TestEnv.ResolveRequired<IEmitterDriverT<TPayload, TMsg>>();
        // THEN
        Assert.NotNull(driver);
    }

}