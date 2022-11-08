using DotCart.Behavior;
using DotCart.Drivers.EventStoreDB.Interfaces;
using DotCart.Effects;
using DotCart.Effects.Drivers;
using DotCart.Schema;
using DotCart.TestEnv.Engine;
using DotCart.TestEnv.Engine.Schema;
using DotCart.TestKit;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.EventStoreDB.Tests;

public class ESDBEventStoreDriverTests : IoCTests
{
    
    private IEventStoreDriver _eventStoreDriver;
    private EventStreamGenerator<SimpleEngineID, Engine> _newEventStream;
    private ScenarioGenerator<SimpleEngineID, Engine> _newScenario;
    private NewState<Engine> _newEngine;
    private IAggregate _aggregate;
    private ICmdHandler _cmdHandler;
    private NewSimpleID<SimpleEngineID> _newID;
    private IAggregateBuilder _aggregateBuilder;

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
        var es = Container.GetRequiredService<EventStreamGenerator<SimpleEngineID, Engine>>();
        // THEN
        Assert.NotNull(es);
    }

    [Fact]
    public void ShouldResolveInitializeEngineScenarioGenerator()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var scGen = Container.GetRequiredService<ScenarioGenerator<SimpleEngineID, Engine>>();
        // THEN
        Assert.NotNull(scGen);
    }

    [Fact]
    public void ShouldGenerateInitializeEngineScenario()
    {
        // GIVEN
        Assert.NotNull(_newScenario);
        // WHEN
        var cmds = _newScenario(_newID(), _newEngine );
        // THEN
        Assert.NotNull(cmds);
        Assert.NotEmpty(cmds);
    }

    
    // TODO: Implement TEST ShouldExecuteInitializeEngineScenario() 
    [Fact]
    public async Task ShouldExecuteInitializeEngineScenario()
    {
        // GIVEN
        Assert.NotNull(_newScenario);
        Assert.NotNull(_cmdHandler);
        // AND
        var cmds = _newScenario(_newID(), _newEngine );
        // WHEN
        var res = true;
        foreach (var cmd in cmds)
        {
            var fbk = await _cmdHandler.Handle(cmd);
            res = res && fbk.IsSuccess;
            Assert.NotNull(fbk);
            if (fbk.IsSuccess) continue;
            Output.WriteLine(fbk.ErrState.ToString());
            break;
        }
        // THEN
        Assert.True(res);
    }


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


    [Fact]
    public void ShouldSaveAggregate()
    {
        // GIVEN 
        Assert.NotNull(_aggregate);
        Assert.NotNull(_eventStoreDriver);
        // WHEN
        var res =  _eventStoreDriver.SaveAsync(_aggregate);
        // THEN
        Assert.NotNull(res);
    }
    

    public ESDBEventStoreDriverTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }
    
    [Fact]
    public async Task ShouldAppendEvents()
    {
        // GIVEN 
        Assert.NotNull(_eventStoreDriver);
        Assert.NotNull(_newEventStream);
        // WHEN
        var ID = _newID();
        var events = _newEventStream(ID, _newEngine);
        var res = await _eventStoreDriver.AppendEventsAsync(ID, events);
        // THEN
        Assert.NotNull(res);
    }


    // [Fact]
    // public async Task ShouldLoadEvents()
    // {
    //     // GIVEN
    //     Assert.NotNull(_eventStoreDriver);
    //     // WHEN
    //     var ID = _newID();
    //     var events = _newEventStream(ID, _newEngine);
    //     var res = await _eventStoreDriver.AppendEventsAsync(ID, events);
    //     var readEvents = await _eventStoreDriver.ReadEventsAsync(ID);
    //     // THEN
    //     Assert.Equal(events.Count(), readEvents.Count());
    //
    // }
    
    
    
    
    

    [Fact]
    public void ShouldSerializeEvents()
    {
        // GIVEN
        Dictionary<string,byte[]> serializedEvents = new();
        var ID = _newID();
        var events = _newEventStream(ID, _newEngine);
        // WHEN
        foreach (var evt in events)
        {
            var s = SerializationHelper.Serialize(evt);
            serializedEvents.Add(evt.GetType().AssemblyQualifiedName, s);
        }
        // THEN
        Assert.NotEmpty(serializedEvents);
    }


    protected override void Initialize()
    {
        _eventStoreDriver = Container.GetRequiredService<IEventStoreDriver>();
        _newEventStream = Container.GetRequiredService<EventStreamGenerator<SimpleEngineID, Engine>>();
        _newScenario = Container.GetRequiredService<ScenarioGenerator<SimpleEngineID, Engine>>();
        _newEngine = Container.GetRequiredService<NewState<Engine>>();
        _newID = Container.GetRequiredService<NewSimpleID<SimpleEngineID>>();
        _cmdHandler = Container.GetRequiredService<ICmdHandler>();
        _aggregateBuilder = Container.GetRequiredService<IAggregateBuilder>();
        _aggregate = _aggregateBuilder.Build();
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
            .AddStartBehavior()
            .AddInitializeBehavior()
            .AddThrottleUpBehavior()
//            .AddConfiguredESDBClients()
            .AddSingleton(_ => A.Fake<IESDBEventSourcingClient>())
            .AddSingleton<IAggregateStoreDriver, ESDBEventStoreDriver>()
            .AddSingleton<IEventStoreDriver, ESDBEventStoreDriver>();
    }
}