using DotCart.Context.Actors;
using DotCart.Context.Spokes;
using DotCart.Core;
using DotCart.Drivers.Default;
using DotCart.Drivers.Redis;
using DotCart.TestKit;
using Engine.Behavior;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize;

public class SpokeTests : IoCTests
{
    private ISpokeBuilder<Context.Initialize.Spoke> _builder;
    private Context.Initialize.Spoke _spoke;

    public SpokeTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


    [Fact]
    public void ShouldResolveSpoke()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _spoke = TestEnv.ResolveHosted<Context.Initialize.Spoke>();
        // THEN
        Assert.NotNull(_spoke);
    }

    [Fact]
    public void ShouldResolveSpokeBuilder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _builder = TestEnv.ResolveRequired<ISpokeBuilder<Context.Initialize.Spoke>>();
        // THEN
        Assert.NotNull(_builder);
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