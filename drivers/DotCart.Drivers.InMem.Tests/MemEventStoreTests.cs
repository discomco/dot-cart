using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
using DotCart.TestKit;
using Engine.Context.ChangeRpm;
using Engine.Context.Common;
using Engine.Context.Initialize;
using Engine.Context.Start;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.InMem.Tests;

public class MemEventStoreTests : IoCTests
{
    private IAggregate _agg;
    private IMemEventStore? _aggStore;
    private IID _engineId;
    private NewState<Engine.Context.Common.Schema.Engine> _newEngine;
    private NewID<Schema.EngineID> _newEngineID;
    private IProjectorDriver _projectorDriver;

    public MemEventStoreTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    // [Fact]
    // public void ShoudResolveProjectorDriver()
    // {
    //     // GIVEN
    //     Assert.NotNull(TestEnv);
    //     // WHEN
    //     _projectorDriver = TestEnv.GetRequiredService<IProjectorDriver>();
    //     // THEN
    //     Assert.NotNull(_projectorDriver);
    // }

    [Fact]
    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var newEngineID = TestEnv.ResolveRequired<NewID<Schema.EngineID>>();
        // THEN
        Assert.NotNull(newEngineID);
    }


    [Fact]
    public void ShouldResolveMemEventStore()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var me = TestEnv.Resolve<IMemEventStore>();
        // THEN
        Assert.NotNull(me);
    }


    [Fact]
    public async Task<IMemEventStore> ShouldSaveAggregate()
    {
        // GIVEN
        Assert.NotNull(_aggStore);
        Assert.NotNull(_agg);
        Assert.NotNull(_engineId);
        Assert.NotNull(_newEngine);
        // AND
        var cmds = ScenariosAndStreams.InitializeScenario(_engineId);
        Assert.NotEmpty(cmds);
        _agg.SetID(_engineId);
        var fbks = await ScenariosAndStreams.RunScenario(_agg, cmds);
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
        _newEngine = TestEnv.ResolveRequired<NewState<Engine.Context.Common.Schema.Engine>>();
        _aggStore = TestEnv.ResolveRequired<IMemEventStore>();
        var builder = TestEnv.ResolveRequired<IAggregateBuilder>();
        _agg = builder.Build();
        _newEngineID = TestEnv.ResolveRequired<NewID<Schema.EngineID>>();
        _engineId = _newEngineID();
    }


    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddModelAggregate()
            .AddStartBehavior()
            .AddInitializeBehavior()
            .AddChangeRpmBehavior()
            .AddStartOnInitializedPolicy()
            .AddCmdHandler()
            .AddMemEventStore();
    }
}