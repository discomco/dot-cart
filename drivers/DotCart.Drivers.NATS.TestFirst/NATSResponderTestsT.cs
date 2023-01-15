using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using NATS.Client;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.Drivers.NATS.TestFirst;

public abstract class NATSResponderTestsT<TSpoke, TResponder, TPayload, TMeta>
    : ResponderTestsT<TSpoke, TResponder, TPayload, TMeta>
    where TPayload : IPayload
    where TMeta : IMetaB
    where TSpoke : ISpokeT<TSpoke>
    where TResponder : IActorT<TSpoke>
{
    protected INatsClientConnectionFactory _conFact;

    public NATSResponderTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveOptionsAction()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var oa = TestEnv.ResolveRequired<Action<Options>>();
        // THEN
        Assert.NotNull(oa);
    }

    [Fact]
    public override async Task ShouldResolveConnectionFactory()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _conFact = TestEnv.ResolveRequired<INatsClientConnectionFactory>();
        // THEN
        Assert.NotNull(_conFact);
    }
}