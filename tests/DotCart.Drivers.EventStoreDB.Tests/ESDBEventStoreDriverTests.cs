using DotCart.Behavior;
using DotCart.Drivers.EventStoreDB.Interfaces;
using DotCart.Effects;
using DotCart.Effects.Drivers;
using DotCart.Schema;
using DotCart.TestEnv.Engine;
using DotCart.TestEnv.Engine.Schema;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.EventStoreDB.Tests;

public class ESDBEventStoreDriverTests : IoCTests
{
    
    private IEventStoreDriver _driver;
    private EventStreamGenerator<EngineID, Engine> _eventStreamGenerator;
    private ScenarioGenerator<EngineID, Engine> _scenarioGenerator;
    private NewState<Engine> _newEngine;
    private IAggregate _aggregate;
    private ICmdHandler _cmdHandler;

    [Fact]
    public void ShouldResolveESDBWriterClient()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var esClient = Container.GetRequiredService<IESDBEventSourcingClient>();
        // THEN
        Assert.NotNull(esClient);
    }
    
    
    [Fact]
    public void ShouldResolveESDBDriver()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var driver = Container.GetRequiredService<IEventStoreDriver>();
        // THEN
        Assert.NotNull(driver);
        Assert.IsType<ESDBEventStoreDriver>(driver);
    }

    [Fact]
    public void ShouldResolveAggregateBuilder()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var ab = Container.GetRequiredService<IAggregateBuilder>();
        // THEN
        Assert.NotNull(ab);
    }

    [Fact]
    public void ShouldResolveAggregate()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var agg = Container.GetRequiredService<IAggregate>();
        // THEN
        Assert.NotNull(agg);
    }
    
    
    [Fact]
    public void ShouldResolveInitializeEngineWithThrottleUpEventStream()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var es = Container.GetRequiredService<EventStreamGenerator<EngineID, Engine>>();
        // THEN
        Assert.NotNull(es);
    }

    [Fact]
    public void ShouldResolveInitializeEngineScenarioGenerator()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var scGen = Container.GetRequiredService<ScenarioGenerator<EngineID, Engine>>();
        // THEN
        Assert.NotNull(scGen);
    }

    [Fact]
    public void ShouldGenerateInitializeEngineScenario()
    {
        // GIVEN
        Assert.NotNull(_scenarioGenerator);
        // WHEN
        var cmds = _scenarioGenerator(EngineID.New, _newEngine );
        // THEN
        Assert.NotNull(cmds);
        Assert.NotEmpty(cmds);
    }

    
    // TODO: Implement TEST ShouldExecuteInitializeEngineScenario() 
    // [Fact]
    // public async Task ShouldExecuteInitializeEngineScenario()
    // {
    //     // GIVEN
    //     Assert.NotNull(_scenarioGenerator);
    //     Assert.NotNull(_cmdHandler);
    //     // AND
    //     var cmds = _scenarioGenerator(EngineID.New, _newEngine );
    //     // WHEN
    //     var res = true;
    //     foreach (var cmd in cmds)
    //     {
    //         var fbk = await _cmdHandler.Handle(cmd);
    //         res = res && fbk.IsSuccess;
    //         Assert.NotNull(fbk);
    //         Assert.True(fbk.IsSuccess);
    //     }
    //     // THEN
    //     Assert.True(res);
    // }
    

    [Fact]
    public void ShouldResolveCmdHandler()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var ch = Container.GetRequiredService<ICmdHandler>();
        // THEN
        Assert.NotNull(ch);
    }
    
    

    [Fact]
    public void ShouldResolveEngineCtor()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var ctor = Container.GetRequiredService<NewState<Engine>>();
        // THEN
        Assert.NotNull(ctor);
    }

    // TODO: Implement TEST ShouldSaveAggregate() 
    // [Fact]
    // public void ShouldSaveAggregate()
    // {
    //     Assert.Fail("");
    //     // GIVEN 
    //     Assert.NotNull(_eventStreamGenerator);
    // }
    

    public ESDBEventStoreDriverTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    protected override void Initialize()
    {
        _driver = Container.GetRequiredService<IEventStoreDriver>();
        _eventStreamGenerator = Container.GetRequiredService<EventStreamGenerator<EngineID, Engine>>();
        _scenarioGenerator = Container.GetRequiredService<ScenarioGenerator<EngineID, Engine>>();
        _newEngine = Container.GetRequiredService<NewState<Engine>>();
        _cmdHandler = Container.GetRequiredService<ICmdHandler>();
    }

    protected override void SetTestEnvironment()
    {}

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddCmdHandler()
            .AddStartOnInitializedPolicy()
            .AddInitializeEngineScenario()
            .AddInitializeEngineWithThrottleUpStream()
            .AddEngineAggregate()
            .AddAggregateBuilder()
            .AddESDBEventStoreDriver();
    }
}