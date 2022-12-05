using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Context.Actors;
using DotCart.Context.Behaviors;
using DotCart.Core;
using DotCart.Drivers.EventStoreDB.Interfaces;
using DotCart.Drivers.Mediator;
using DotCart.TestKit;
using DotCart.TestKit.Behavior;
using DotCart.TestKit.Schema;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.EventStoreDB.Tests;

public class ESDBEventStoreTests : IoCTests
{
    private IAggregate? _aggregate;
    private IAggregateBuilder? _aggregateBuilder;
    private ICmdHandler? _cmdHandler;
    private IEventStore? _eventStore;
    private StateCtorT<TheDoc> _newDoc;
    private IDCtorT<TheID> _newID;
    private IExchange _exchange;


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
    public async Task ShouldResolveExchange()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _exchange = TestEnv.ResolveRequired<IExchange>();
        // THEN
        Assert.NotNull(_exchange);
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
        var ctor = TestEnv.ResolveRequired<StateCtorT<TheDoc>>();
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


    protected override void Initialize()
    {
        _eventStore = TestEnv.ResolveRequired<IEventStore>();
        _newDoc = TestEnv.ResolveRequired<StateCtorT<TheDoc>>();
        _newID = TestEnv.ResolveRequired<IDCtorT<TheID>>();
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
            .AddTransient<IAggregate, TheAggregate>()
            .AddAggregateBuilder()
            .AddTransient(_ => TheDoc.Rand)
            .AddTransient(_ => TheID.Ctor)
            .AddSingleton<IAggregateStore, ESDBStore>()
            .AddSingleton<IEventStore, ESDBStore>()
            .AddSingletonExchange()
            .AddConfiguredESDBClients();
    }
}