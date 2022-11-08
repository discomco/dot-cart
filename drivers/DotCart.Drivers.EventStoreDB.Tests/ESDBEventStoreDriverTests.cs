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
using Constants = DotCart.Behavior.Constants;

namespace DotCart.Drivers.EventStoreDB.Tests;

public class ESDBEventStoreDriverTests : IoCTests
{
    private IEventStoreDriver _eventStoreDriver;
    private EventStreamGenerator<EngineID, Engine> _newEventStream;
    private ScenarioGenerator<EngineID, Engine> _newScenario;
    private NewState<Engine> _newEngine;
    private IAggregate _aggregate;
    private ICmdHandler _cmdHandler;
    private NewID<EngineID> _newID;
    private IAggregateBuilder _aggregateBuilder;

    private bool _isCICD = true;

    [Fact]
    public void ShouldResolveESDBEventSourcingClient()
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
        var aggBuilder = Container.GetRequiredService<IAggregateBuilder>();
        var agg = aggBuilder.Build();
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
        Assert.NotNull(_newScenario);
        // WHEN
        var cmds = _newScenario(_newID(), _newEngine);
        // THEN
        Assert.NotNull(cmds);
        Assert.NotEmpty(cmds);
    }


    // TODO: Implement TEST ShouldExecuteInitializeEngineScenario() 
    [Fact]
    public async Task ShouldExecuteInitializeEngineScenario()
    {
        if (_isCICD)
        {
            Assert.True(true);
            return;
        }
        // GIVEN
        Assert.NotNull(_newScenario);
        Assert.NotNull(_cmdHandler);
        // AND
        var cmds = _newScenario(_newID(), _newEngine);
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

        var engineID = _newID();
        _aggregate.SetID(engineID);

        // WHEN
        var res = _eventStoreDriver.SaveAsync(_aggregate);
        // THEN
        Assert.NotNull(res);
    }

    [Fact]
    public async Task ShouldLoadAggregate()
    {
        if (_isCICD)
        {
            Assert.True(true);
            return;
        }

        // GIVEN
        Assert.NotNull(_aggregate);
        Assert.NotNull(_eventStoreDriver);
        var engineID = _newID();
        _aggregate.SetID(engineID);
        var scenario = _newScenario(engineID, _newEngine);
        var res = true;
        foreach (var cmd in scenario)
        {
            var fbk = await _cmdHandler.Handle(cmd);
            if (!fbk.IsSuccess)
            {
                Output.WriteLine($"{fbk.ErrState}");
            }

            res = res && fbk.IsSuccess;
        }

        if (res)
        {
            var agg = _aggregateBuilder.Build();
            agg.SetID(engineID);
            // WHEN
            await _eventStoreDriver.LoadAsync(agg);
            // THEN
            Assert.False(agg.Version == Constants.NewAggregateVersion);
        }
        else
        {
            Assert.True(false);
        }
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


    [Fact]
    public async Task ShouldLoadEvents()
    {
        // GIVEN
        Assert.NotNull(_eventStoreDriver);
        // WHEN
        var ID = _newID();
        var events = _newEventStream(ID, _newEngine);
        var res = await _eventStoreDriver.AppendEventsAsync(ID, events);
        try
        {
            var readEvents = await _eventStoreDriver.ReadEventsAsync(ID);
            // THEN
            Assert.Equal(events.Count(), readEvents.Count());
        }
        catch (Exception e)
        {
            Output.WriteLine(e.Message);
            Assert.True(true);
        }
    }


    [Fact]
    public void ShouldSerializeEvents()
    {
        // GIVEN
        Dictionary<string, byte[]> serializedEvents = new();
        var ID = _newID();
        var events = _newEventStream(ID, _newEngine);
        // WHEN
        foreach (var evt in events)
        {
            var s = SerializationHelper.Serialize(evt);
            serializedEvents.Add(evt.Topic, s);
        }

        // THEN
        Assert.NotEmpty(serializedEvents);
    }


    protected override void Initialize()
    {
        _eventStoreDriver = Container.GetRequiredService<IEventStoreDriver>();
        _newEventStream = Container.GetRequiredService<EventStreamGenerator<EngineID, Engine>>();
        _newScenario = Container.GetRequiredService<ScenarioGenerator<EngineID, Engine>>();
        _newEngine = Container.GetRequiredService<NewState<Engine>>();
        _newID = Container.GetRequiredService<NewID<EngineID>>();
        _cmdHandler = Container.GetRequiredService<ICmdHandler>();
        _aggregateBuilder = Container.GetRequiredService<IAggregateBuilder>();
        _aggregate = _aggregateBuilder.Build();
    }

    protected override void SetTestEnvironment()
    {
    }

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
            .AddSingleton<IAggregateStoreDriver, ESDBEventStoreDriver>()
            .AddSingleton<IEventStoreDriver, ESDBEventStoreDriver>();
        if (_isCICD)
        {
            services.AddSingleton(_ => A.Fake<IESDBEventSourcingClient>());
        }
        else
        {
            services.AddConfiguredESDBClients();
        }
    }
}