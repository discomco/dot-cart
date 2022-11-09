using DotCart.Schema;
using DotCart.TestEnv.Engine.Schema;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Contract.Tests;

public class HopeTests : IoCTests
{
    private NewState<TestEnv.Engine.Schema.Engine> _newEngine;

    public HopeTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    [Fact]
    public void ShouldCreateInitializeHope()
    {
        // GIVEN
        var engine = _newEngine();
        var pl = TestEnv.Engine.Initialize.Payload.New(engine);
        // WHEN
        var hope = TestEnv.Engine.Initialize.Hope.New(engine.Id, pl.ToBytes());
        // THEN
        Assert.NotNull(hope);
    }

    protected override void Initialize()
    {
        _newEngine = Container.GetRequiredService<NewState<TestEnv.Engine.Schema.Engine>>();
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddEngineCtor();
    }
}