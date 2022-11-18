using DotCart.Context.Spokes;
using DotCart.Core;
using DotCart.Drivers.EventStoreDB;
using DotCart.Drivers.Serilog;
using DotCart.TestKit;
using Engine.Context.Initialize;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Spokes;

public class InitializeSpokeTests : IoCTests
{
    private ISpokeBuilder<Spoke> _builder;
    private Spoke _spoke;

    public InitializeSpokeTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


    [Fact]
    public void ShouldResolveSpoke()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _spoke = TestEnv.ResolveHosted<Spoke>();
        // THEN
        Assert.NotNull(_spoke);
    }

    [Fact]
    public void ShouldResolveSpokeBuilder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _builder = TestEnv.ResolveRequired<ISpokeBuilder<Spoke>>();
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
            .AddConsoleLogger()
            .AddESDBInfra()
            .AddInitializeSpoke();
    }
}