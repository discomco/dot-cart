using DotCart.Behavior;
using DotCart.Effects;
using DotCart.Schema;
using DotCart.TestEnv.Engine;
using DotCart.TestEnv.Engine.Behavior;
using DotCart.TestEnv.Engine.Schema;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.InMem.Tests;

public class MemEventStoreTests : IoCTests
{
    private IMemEventStore? _aggStore;
    private IAggregate _agg;
    private IID _engineId;
    private NewState<Engine> _newEngine;

    public MemEventStoreTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }


    [Fact]
    public void ShouldResolveMemEventStore()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var me = Container.GetService<IAggregateStore>();
        // THEN
        Assert.NotNull(me);
    }


    [Fact]
    public async Task ShouldSaveAggregate()
    {
        // GIVEN
        Assert.NotNull(_aggStore);
        Assert.NotNull(_agg);
        Assert.NotNull(_engineId);
        Assert.NotNull(_newEngine);
        // AND
        var cmds = HelperFuncs.InitializeScenario(_engineId, _newEngine);
        Assert.NotEmpty(cmds);
        _agg.SetID(_engineId);
        var fbks = await HelperFuncs.RunScenario(_agg, cmds);
        // AND
        Assert.NotEmpty(_agg.UncommittedEvents);
        // WHEN
        _aggStore.Save(_agg);
        // THEN
        Assert.NotEmpty(_aggStore.GetStream(_engineId));
    }

    protected override void Initialize()
    {
        _engineId = EngineID.New;
        _newEngine = Container.GetRequiredService<NewState<Engine>>();
        _aggStore = Container.GetRequiredService<IMemEventStore>();
        var builder = Container.GetRequiredService<IAggregateBuilder>();
        _agg = builder.Build();
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddEngineAggregate()
            .AddMemEventStore();
    }
}