using DotCart.Behavior;
using DotCart.Effects;
using DotCart.Schema;
using DotCart.TestEnv.Engine;
using DotCart.TestEnv.Engine.Schema;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.InMem.Tests;

public class MemEventStoreTests : IoCTests
{
    private IAggregate _agg;
    private IMemEventStoreDriver? _aggStore;
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
        var me = Container.GetService<IMemEventStoreDriver>();
        // THEN
        Assert.NotNull(me);
    }


    [Fact]
    public async Task<IMemEventStoreDriver> ShouldSaveAggregate()
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
        await _aggStore.SaveAsync(_agg).ConfigureAwait(false);

        //     Assert.NotEmpty(_agg.UncommittedEvents);
        // THEN
        Assert.NotEmpty(_aggStore.GetStream(_engineId));
        return _aggStore;
    }

    [Fact]
    public async Task ShouldLoadAggregate()
    {
        // GIVEN
        _aggStore = await ShouldSaveAggregate();
        // WHEN
        await _aggStore.LoadAsync(_agg);
        // THEN
    }

    protected override void Initialize()
    {
        _engineId = EngineID.New;
        _newEngine = Container.GetRequiredService<NewState<Engine>>();
        _aggStore = Container.GetRequiredService<IMemEventStoreDriver>();
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
            .AddStartOnInitializedPolicy()
            .AddCmdHandler()
            .AddMemEventStore();
    }
}