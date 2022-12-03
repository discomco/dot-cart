using DotCart.Core;
using DotCart.Drivers.Default;
using DotCart.TestFirst.Delivery;
using DotCart.TestKit;
using Engine.Behavior;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize;

[Name(Context.Initialize.SpokeName)]
public class SpokeTests : SpokeTestsT<Context.Initialize.Spoke>
{
    public SpokeTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void Initialize()
    {
    }

    protected override void SetTestEnvironment()
    {
        DotEnv.FromEmbedded();
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddDefaultDrivers<Behavior.Engine, IEngineSubscriptionInfo>()
            .AddInitializeSpoke();
    }
}