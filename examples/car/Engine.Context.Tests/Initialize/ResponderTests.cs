using DotCart.TestFirst.Actors;
using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize;

public class ResponderTests 
    : NATSResponderTestsT<
        Contract.Initialize.Hope,
        Behavior.Initialize.Cmd>
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
            // .AddDefaultDrivers<Behavior.Engine, IEngineSubscriptionInfo>()
            // .AddSingleton(_ => A.Fake<IAggregateStore>())
            .AddInitializeSpoke();
    }

 
}