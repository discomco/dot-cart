using DotCart.Context.Spokes;
using DotCart.Core;
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

    public InitializeSpokeTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }


    [Fact]
    public void ShouldResolveSpoke()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        _spoke = Container.GetHostedService<Spoke>();
        // THEN
        Assert.NotNull(_spoke);
    }

    [Fact]
    public void ShouldResolveSpokeBuilder()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        _builder = Container.GetRequiredService<ISpokeBuilder<Spoke>>();
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
            .AddESDBInfra<Spoke>()
            .AddInitializeSpoke();
    }
}