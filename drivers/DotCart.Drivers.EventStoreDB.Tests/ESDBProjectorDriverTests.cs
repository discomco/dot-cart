using DotCart.Context.Abstractions;
using DotCart.Context.Abstractions.Drivers;
using DotCart.Core;
using DotCart.Drivers.EventStoreDB.Interfaces;
using DotCart.Drivers.InMem;
using DotCart.Drivers.Serilog;
using DotCart.TestKit;
using Engine.Context.ChangeRpm;
using Engine.Context.Common;
using Engine.Context.Common.Drivers;
using Engine.Context.Common.Effects;
using Engine.Context.Start;
using Engine.Contract.Schema;
using EventStore.Client;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit.Abstractions;
using Spoke = Engine.Context.Initialize.Spoke;

namespace DotCart.Drivers.EventStoreDB.Tests;

public class ESDBProjectorDriverTests : IoCTests
{
    private IESDBPersistentSubscriptionsClient _client;
    private IProjectorDriver _driver;
    private IEventStoreDriver _eventStore;
    private IExchange _exchange;
    private IHostExecutor _executor;
    private IESDBEngineEventFeeder _feeder;
    private ILogger _logger;
    private IModelStore<Engine.Context.Common.Schema.Engine> _memStore;
    private EventStreamGenerator<EngineID> _newEventStream;
    private SubscriptionFilterOptions _subOptions;

    public ESDBProjectorDriverTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


    [Fact]
    public Task ShouldResolveProjectorDriver()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _driver = TestEnv.GetRequiredService<IProjectorDriver<IEngineSubscriptionInfo>>();
        // THEN
        Assert.NotNull(_driver);
        return Task.CompletedTask;
    }

    [Fact]
    public Task ShouldResolveSubscriptionFilterOptions()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _subOptions = TestEnv.GetRequiredService<SubscriptionFilterOptions>();
        // THEN
        Assert.NotNull(_subOptions);
        return Task.CompletedTask;
    }


    [Fact]
    public Task ShouldResolveHostExecutor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _executor = TestEnv.GetRequiredService<IHostExecutor>();
        // THEN
        Assert.NotNull(_executor);
        return Task.CompletedTask;
    }

    [Fact]
    public async Task ShouldStartFeeder()
    {
        if (TestKit.Config.IsPipeline)
        {
            Assert.True(true);
            return;
        }

        // GIVEN
        Assert.NotNull(_feeder);
        // WHEN
        var source = new CancellationTokenSource(1_000);

        var token = source.Token;

        await _executor.StartAsync(token);
        // THEN
        while (!token.IsCancellationRequested)
        {
        }

        Assert.True(true);
    }


    [Fact]
    public void ShouldResolveExchange()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _exchange = TestEnv.GetRequiredService<IExchange>();
        // THEN
        Assert.NotNull(_exchange);
    }


    // [Fact]
    // public async Task ShouldPublishToMediator()
    // {
    //     // GIVEN
    //     Assert.NotNull(_feeder);
    //     Assert.NotNull(_memStore);
    //     // WHEN
    //     var source = new CancellationTokenSource(2_000);
    //     var token = source.Token;
    //     await _executor.StartAsync(token);
    //     while (!token.IsCancellationRequested)
    //     {
    //         Thread.Sleep(100);
    //         var hasData = await _memStore.HasData();
    //         Output.WriteLine($"Data Received: {hasData}");
    //     }
    //     Assert.True(await _memStore.HasData());
    // }

    [Fact]
    public void ShouldResolveEventStoreDriver()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _eventStore = TestEnv.GetRequiredService<IEventStoreDriver>();
        // THEN
        Assert.NotNull(_eventStore);
    }

    [Fact]
    public Task ShouldResolveLogger()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _logger = TestEnv.GetRequiredService<ILogger>();
        // THEN
        Assert.NotNull(_logger);
        return Task.CompletedTask;
    }


    [Fact]
    public void ShouldResolveESDBPersistentSubscriptionsClient()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _client = TestEnv.GetRequiredService<IESDBPersistentSubscriptionsClient>();
        // THEN
        Assert.NotNull(_client);
    }

    [Fact]
    public void ShouldResolveESDBEngineEventFeeder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _feeder = TestEnv.GetRequiredService<IESDBEngineEventFeeder>();
        // THEN 
        Assert.NotNull(_feeder);
    }

    [Fact]
    public void ShouldResolveEngineEventStreamGenerator()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newEventStream = TestEnv.GetRequiredService<EventStreamGenerator<EngineID>>();
        // THEN
        Assert.NotNull(_newEventStream);
    }


    protected override void Initialize()
    {
        _executor = TestEnv.GetRequiredService<IHostExecutor>();
        _feeder = TestEnv.GetRequiredService<IESDBEngineEventFeeder>();
        _newEventStream = TestEnv.GetRequiredService<EventStreamGenerator<EngineID>>();
        _memStore = TestEnv.GetRequiredService<IModelStore<Engine.Context.Common.Schema.Engine>>();
    }

    protected override void SetTestEnvironment()
    {
        DotEnv.FromEmbedded();
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddInitializeEngineWithThrottleUpStream()
            .AddESDBEngineEventFeeder()
            .AddEngineESDBProjectorDriver<Spoke>()
            .AddStartedProjections()
            .AddChangeRpmProjections()
            .AddConsoleLogger();

        if (TestKit.Config.IsPipeline)
            services
                .AddMemEventStore()
                .AddSingleton(_ => A.Fake<IESDBPersistentSubscriptionsClient>());
        else
            services
                .AddConfiguredESDBClients()
                .AddESDBEventStoreDriver();
    }
}