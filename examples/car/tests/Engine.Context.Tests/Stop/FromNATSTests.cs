using DotCart.Abstractions.Behavior;
using DotCart.Drivers.NATS.TestFirst;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Stop;

public class FromNATSTests
    : NATSResponderTestsT<Context.Stop.Spoke, Context.Stop.FromNATS, Contract.Stop.Payload, MetaB>
{
    public FromNATSTests(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    protected override void Initialize()
    {
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddStopSpoke();
    }
}