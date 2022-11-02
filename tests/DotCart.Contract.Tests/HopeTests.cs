using DotCart.Schema;
using DotCart.TestEnv.Engine;
using DotCart.TestEnv.Engine.Schema;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Contract.Tests;

public class HopeTests: IoCTests
{
    private NewState<Engine> _newEngine;

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

    public HopeTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    protected override void Initialize()
    {
        _newEngine = Container.GetRequiredService<NewState<Engine>>();
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddSingleton(Engine.Ctor);
    }
}