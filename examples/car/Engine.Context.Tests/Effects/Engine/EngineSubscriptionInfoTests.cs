using DotCart.TestFirst;
using DotCart.TestKit;
using Engine.Context.Common.Drivers;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Effects.Engine;

public class EngineSubscriptionInfoTests : SubscriptionInfoTests<IEngineSubscriptionInfo>
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