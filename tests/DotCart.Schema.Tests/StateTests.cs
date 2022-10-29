using System.Text.Json;
using DotCart.TestEnv.Engine.Schema;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Schema.Tests;

public class StateTests : IoCTests
{
    public StateTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    [Fact]
    public void ShouldCreateANewState()
    {
        // GIVEN
        var newState = Container.GetService<NewState<Engine>>();
        // WHEN
        var eng = newState();
        eng.Id = EngineID.New.Value;
        // THEN
        Assert.NotNull(eng.Details);
    }

    [Fact]
    public void ShouldCreateDifferentStates()
    {
        // GIVEN
        var newState = Container.GetService<NewState<Engine>>();
        // WHEN
        var eng1 = newState();
        var eng2 = newState();
        // THEN
        Assert.NotNull(eng1.Details);
        Assert.NotNull(eng2.Details);
        Assert.NotEqual(eng1, eng2);
    }

    [Fact]
    public void ShouldStateBeSerializable()
    {
        // GIVEN
        var newState = Container.GetService<NewState<Engine>>();
        var eng = newState();
        eng.Id = EngineID.New.Value;
        // WHEN
        var s = JsonSerializer.Serialize(eng);
        // THEN
        Assert.NotEmpty(s);
    }

    [Fact]
    public void ShouldStateBeDeserializable()
    {
        // GIVEN
        var s = "{\"Id\":\"engine-20298432-4a89-4aac-8430-9ce1161e55fd\",\"Status\":1}";
        // WHEN
        var eng = JsonSerializer.Deserialize<Engine>(s);
        // THEN
        Assert.Equal("engine-20298432-4a89-4aac-8430-9ce1161e55fd", eng.Id);
        Assert.Equal(EngineStatus.Initialized, eng.Status);
    }

    protected override void Initialize()
    {
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services.AddSingleton(Engine.Ctor);
    }
}