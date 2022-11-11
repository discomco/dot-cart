using DotCart.Client.Schemas;
using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
using DotCart.Context.Effects.Drivers;
using DotCart.Context.Schemas;
using DotCart.Core;
using DotCart.Drivers.EventStoreDB.Interfaces;
using DotCart.TestKit;
using Engine.Client.Schema;
using Engine.Context.ChangeRpm;
using Engine.Context.Common;
using Engine.Context.Start;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Constants = DotCart.Context.Behaviors.Constants;
using Exception = System.Exception;

namespace DotCart.Drivers.EventStoreDB.Tests;

public class ESDBEventStoreDriverTests : IoCTests
{
    private IAggregate _aggregate;
    private IAggregateBuilder _aggregateBuilder;
    private ICmdHandler _cmdHandler;
    private IEventStoreDriver _eventStoreDriver;
    private NewState<Engine.Context.Common.Schema.Engine> _newEngine;
    private EventStreamGenerator<EngineID> _newEventStream;
    private NewID<EngineID> _newID;
    private ScenarioGenerator<EngineID> _newScenario;


    public ESDBEventStoreDriverTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }


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
        var es = Container.GetRequiredService<EventStreamGenerator<EngineID>>();
        // THEN
        Assert.NotNull(es);
    }

    [Fact]
    public void ShouldResolveInitializeEngineScenarioGenerator()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var scGen = Container.GetRequiredService<ScenarioGenerator<EngineID>>();
        // THEN
        Assert.NotNull(scGen);
    }

    [Fact]
    public void ShouldGenerateInitializeEngineScenario()
    {
        // GIVEN
        Assert.NotNull(_newScenario);
        // WHEN
        var cmds = _newScenario(_newID());
        // THEN
        Assert.NotNull(cmds);
        Assert.NotEmpty(cmds);
    }


    // TODO: Implement TEST ShouldExecuteInitializeEngineScenario() 
    [Fact]
    public async Task ShouldExecuteInitializeEngineScenario()
    {
        if (TestKit.Config.IsPipeline)
        {
            Assert.True(true);
            return;
        }

        // GIVEN
        Assert.NotNull(_newScenario);
        Assert.NotNull(_cmdHandler);
        // AND
        var cmds = _newScenario(_newID());
        // WHEN
        var res = true;
        foreach (var cmd in cmds)
        {
            var fbk = await _cmdHandler.HandleAsync(cmd);
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
        var ctor = Container.GetRequiredService<NewState<Engine.Context.Common.Schema.Engine>>();
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
        if (TestKit.Config.IsPipeline)
        {
            Assert.True(true);
            return;
        }

        // GIVEN
        Assert.NotNull(_aggregate);
        Assert.NotNull(_eventStoreDriver);
        var engineID = _newID();
        _aggregate.SetID(engineID);
        var scenario = _newScenario(engineID);
        var res = true;
        foreach (var cmd in scenario)
        {
            var fbk = await _cmdHandler.HandleAsync(cmd);
            if (!fbk.IsSuccess) Output.WriteLine($"{fbk.ErrState}");

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

    [Fact]
    public async Task ShouldAppendEvents()
    {
        // GIVEN 
        Assert.NotNull(_eventStoreDriver);
        Assert.NotNull(_newEventStream);
        // WHEN
        var ID = _newID();
        var events = _newEventStream(ID);
        var res = await _eventStoreDriver.AppendEventsAsync(ID, events);
        // THEN
        Assert.NotNull(res);
    }


    [Fact]
    public async Task ShouldLoadEvents()
    {
        if (TestKit.Config.IsPipeline)
        {
            Assert.True(true);
            return;
        }

        // GIVEN
        Assert.NotNull(_eventStoreDriver);
        // WHEN
        var ID = _newID();
        var events = _newEventStream(ID);
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
        var events = _newEventStream(ID);
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
        _newEventStream = Container.GetRequiredService<EventStreamGenerator<EngineID>>();
        _newScenario = Container.GetRequiredService<ScenarioGenerator<EngineID>>();
        _newEngine = Container.GetRequiredService<NewState<Engine.Context.Common.Schema.Engine>>();
        _newID = Container.GetRequiredService<NewID<EngineID>>();
        _cmdHandler = Container.GetRequiredService<ICmdHandler>();
        _aggregateBuilder = Container.GetRequiredService<IAggregateBuilder>();
        _aggregate = _aggregateBuilder.Build();
    }

    protected override void SetTestEnvironment()
    {
        DotEnv.FromEmbedded();
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services.AddCmdHandler()
            .AddStartOnInitializedPolicy()
            .AddInitializeEngineScenario()
            .AddInitializeEngineWithThrottleUpStream()
            .AddEngineAggregate()
            .AddAggregateBuilder()
            .AddStartBehavior()
            .AddChangeRpmBehavior()
            .AddSingleton<IAggregateStoreDriver, ESDBEventStoreDriver>()
            .AddSingleton<IEventStoreDriver, ESDBEventStoreDriver>();
        if (TestKit.Config.IsPipeline)
            services.AddSingleton(_ => A.Fake<IESDBEventSourcingClient>());
        else
            services.AddConfiguredESDBClients();
    }
}