using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;
using DotCart.Logging;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.Drivers.NATS.TestFirst;

public abstract class NATSRequesterTestsT<TPayload>
    : NATSTestsB
    where TPayload : IPayload
{


    [Fact]
    public void ShouldRequestHaveHopeTopic()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var topic = HopeTopicAtt.Get<TPayload>();
        // THEN
        Assert.NotEmpty(topic);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public void ShouldResolveNATSRequester()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var requester = TestEnv.ResolveRequired<INATSRequesterT<TPayload>>();
        // THEN
        Assert.NotNull(requester);
    }


    protected NATSRequesterTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }


    protected override void InjectDependencies(IServiceCollection services)
    {
        base.InjectDependencies(services);
        services
            .AddNATSRequesterT<TPayload>();
    }
}