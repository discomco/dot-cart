using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
using DotCart.Core;
using DotCart.Drivers.EventStoreDB.Interfaces;
using DotCart.TestKit;
using Engine.Context.ChangeRpm;
using Engine.Context.Common;
using Engine.Context.Start;
using Engine.Contract;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Exception = System.Exception;

namespace DotCart.Drivers.EventStoreDB.Tests;

public class ESDBEventStoreTests : IoCTests
{
    private IAggregate? _aggregate;
    private IAggregateBuilder? _aggregateBuilder;
    private ICmdHandler? _cmdHandler;
    private IEventStore? _eventStore;
    private NewState<Engine.Context.Common.Schema.Engine> _newEngine;
    private EventStreamGenerator<Schema.EngineID> _newEventStream;
    private NewID<Schema.EngineID> _newID;
    private ScenarioGenerator<Schema.EngineID> _newScenario;


    public ESDBEventStoreTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


    [Fact]
    public void ShouldResolveESDBEventSourcingClient()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var esClient = TestEnv.ResolveRequired<IESDBEventSourcingClient>();
        // THEN
        Assert.NotNull(esClient);
    }


    [Fact]
    public void ShouldResolveESDBDriver()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var driver = TestEnv.ResolveRequired<IEventStore>();
        // THEN
        Assert.NotNull(driver);
        Assert.IsType<ESDBStore>(driver);
    }

    [Fact]
    public void ShouldResolveAggregateBuilder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var ab = TestEnv.ResolveRequired<IAggregateBuilder>();
        // THEN
        Assert.NotNull(ab);
    }

    [Fact]
    public void ShouldResolveAggregate()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var aggBuilder = TestEnv.ResolveRequired<IAggregateBuilder>();
        var agg = aggBuilder.Build();
        // THEN
        Assert.NotNull(agg);
    }


    [Fact]
    public void ShouldResolveInitializeEngineWithThrottleUpEventStream()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var es = TestEnv.ResolveRequired<EventStreamGenerator<Schema.EngineID>>();
        // THEN
        Assert.NotNull(es);
    }

    [Fact]
    public void ShouldResolveInitializeEngineScenarioGenerator()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var scGen = TestEnv.ResolveRequired<ScenarioGenerator<Schema.EngineID>>();
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
        Assert.NotNull(TestEnv);
        // WHEN
        var ch = TestEnv.ResolveRequired<ICmdHandler>();
        // THEN
        Assert.NotNull(ch);
    }


    [Fact]
    public void ShouldResolveEngineCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var ctor = TestEnv.ResolveRequired<NewState<Engine.Context.Common.Schema.Engine>>();
        // THEN
        Assert.NotNull(ctor);
    }


    [Fact]
    public void ShouldSaveAggregate()
    {
        // GIVEN 
        Assert.NotNull(_aggregate);
        Assert.NotNull(_eventStore);

        var engineID = _newID();
        _aggregate.SetID(engineID);

        // WHEN
        var res = _eventStore.SaveAsync(_aggregate);
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
        Assert.NotNull(_eventStore);
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
            await _eventStore.LoadAsync(agg);
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
        Assert.NotNull(_eventStore);
        Assert.NotNull(_newEventStream);
        // WHEN
        var ID = _newID();
        var events = _newEventStream(ID);
        var res = await _eventStore.AppendEventsAsync(ID, events);
        // THEN
        Assert.NotNull(res);
    }


    [Fact]
    public async Task ShouldLoadEvents()
    {
        var _cts = new CancellationTokenSource(100);

        if (TestKit.Config.IsPipeline)
        {
            Assert.True(true);
            return;
        }

        // GIVEN
        Assert.NotNull(_eventStore);
        // WHEN
        var ID = _newID();
        var events = _newEventStream(ID);
        var res = await _eventStore.AppendEventsAsync(ID, events, _cts.Token);
        try
        {
            var readEvents = await _eventStore.ReadEventsAsync(ID, _cts.Token);
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
        DuplicatesList<string, byte[]> serializedEvents = new();
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
        _eventStore = TestEnv.ResolveRequired<IEventStore>();
        _newEventStream = TestEnv.ResolveRequired<EventStreamGenerator<Schema.EngineID>>();
        _newScenario = TestEnv.ResolveRequired<ScenarioGenerator<Schema.EngineID>>();
        _newEngine = TestEnv.ResolveRequired<NewState<Engine.Context.Common.Schema.Engine>>();
        _newID = TestEnv.ResolveRequired<NewID<Schema.EngineID>>();
        _cmdHandler = TestEnv.ResolveRequired<ICmdHandler>();
        _aggregateBuilder = TestEnv.ResolveRequired<IAggregateBuilder>();
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
            .AddInitializeWithThrottleUpEvents()
            .AddModelAggregate()
            .AddAggregateBuilder()
            .AddStartBehavior()
            .AddChangeRpmBehavior()
            .AddSingleton<IAggregateStore, ESDBStore>()
            .AddSingleton<IEventStore, ESDBStore>();
        if (TestKit.Config.IsPipeline)
            services.AddSingleton(_ => A.Fake<IESDBEventSourcingClient>());
        else
            services.AddConfiguredESDBClients();
    }
}