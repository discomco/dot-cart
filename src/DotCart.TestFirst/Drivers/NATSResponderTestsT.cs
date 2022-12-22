using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using NATS.Client;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Drivers;

public abstract class NATSResponderTestsT<THope, TCmd> : ResponderTestsT<THope, TCmd>
    where TCmd : ICmdB
    where THope : IHopeB
{
    protected IEncodedConnection _bus;

    public NATSResponderTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public override async Task ShouldResolveConnection()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _bus = TestEnv.ResolveRequired<IEncodedConnection>();
        // THEN
        Assert.NotNull(_bus);
    }
}