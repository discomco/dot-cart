using DotCart.Abstractions.Behavior;
using DotCart.Drivers.NATS.TestFirst;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize;

public class FromNATSTests
    : NATSResponderTestsT<Context.Initialize.Spoke, Context.Initialize.FromNATS, Contract.Initialize.Payload, MetaB>
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
            // .AddDefaultDrivers<Contract.Schema.Engine, IEngineSubscriptionInfo>()
            // .AddSingleton(_ => A.Fake<IAggregateStore>())
            .AddInitializeSpoke();
    }
}