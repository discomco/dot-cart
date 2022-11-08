using DotCart.Drivers.EventStoreDB.Tests;
using DotCart.TestEnv.Engine.Drivers;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Effects.Tests.Engine;

public class EngineSubscriptionInfoTests: SubscriptionInfoTests<IEngineSubscriptionInfo>
{
    public EngineSubscriptionInfoTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    protected override void Initialize()
    {
        
    }

    protected override void SetTestEnvironment()
    {
        
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        
    }
}