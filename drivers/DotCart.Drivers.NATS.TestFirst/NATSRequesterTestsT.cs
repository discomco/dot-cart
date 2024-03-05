using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.Drivers.NATS.TestFirst;

public abstract class NATSRequesterTestsT<TPayload>
    : NATSTestsB
    where TPayload : IPayload
{
    protected NATSRequesterTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }


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


    protected override void InjectDependencies(IServiceCollection services)
    {
        base.InjectDependencies(services);
        services
            .AddNATSRequesterT<TPayload>();
    }
}