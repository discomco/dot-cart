using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize;

public class ResponderTests
    : NATSResponderTestsT<
        Contract.Initialize.Payload,
        EventMeta>
{
    public ResponderTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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