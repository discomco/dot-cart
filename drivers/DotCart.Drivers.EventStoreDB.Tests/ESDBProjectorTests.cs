using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Actors;
using DotCart.Core;
using DotCart.Logging;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using DotCart.TestKit.Mocks;
using EventStore.Client;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit.Abstractions;

namespace DotCart.Drivers.EventStoreDB.Tests;

public class ESDBProjectorTests(ITestOutputHelper output, IoCTestContainer testEnv)
    : IoCTests(output, testEnv)
{
    private IResilientPersistentSubscriptionsESDBClient? _client;
    private IProjectorDriver? _driver;
    private IEventStore? _eventStore;
    private IExchange? _exchange;
    private IHostExecutor? _executor;
    private ILogger? _logger;
    private IDocStoreT<TheSchema.Doc>? _memStore;
    private SubscriptionFilterOptions? _subOptions;


    [Fact]
    public Task ShouldResolveSubscriptionFilterOptions()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _subOptions = TestEnv.ResolveRequired<SubscriptionFilterOptions>();
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
        _executor = TestEnv.ResolveRequired<IHostExecutor>();
        // THEN
        Assert.NotNull(_executor);
        return Task.CompletedTask;
    }


    [Fact]
    public void ShouldResolveExchange()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _exchange = TestEnv.ResolveRequired<IExchange>();
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
    public void ShouldResolveEventStore()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _eventStore = TestEnv.ResolveRequired<IEventStore>();
        // THEN
        Assert.NotNull(_eventStore);
    }

    [Fact]
    public Task ShouldResolveLogger()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _logger = TestEnv.ResolveRequired<ILogger>();
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
        _client = TestEnv.ResolveRequired<IResilientPersistentSubscriptionsESDBClient>();
        // THEN
        Assert.NotNull(_client);
    }

    [Fact]
    public void ShouldResolveEventFeeder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var feeder = TestEnv.ResolveRequired<IEventFeeder>();
        // THEN
        Assert.NotNull(feeder);
    }

    [Fact]
    public void ShouldResolveStreamGenFunc()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var streamGen = TestEnv.ResolveRequired<EventStreamGenFuncT<TheSchema.DocID>>();
        // THEN
        Assert.NotNull(streamGen);
    }


    [Fact]
    public void ShouldResolveProjector()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var projector = TestEnv.ResolveRequired<IProjector>();
        // THEN
        Assert.NotNull(projector);
    }


    [Fact]
    public void ShouldResolveProjectorDriver()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _driver = TestEnv.ResolveRequired<IProjectorDriverT<TheActors.IProjectorInfo>>();
        // THEN
        Assert.NotNull(_driver);
    }

    [Fact]
    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var ctor = TestEnv.ResolveRequired<IDCtorT<TheSchema.DocID>>();
        // THEN
        Assert.NotNull(ctor);
    }


    [Fact]
    public async Task ShouldFeedEventsToEventStore()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var feeder = TestEnv.ResolveRequired<IEventFeeder>();
        Assert.NotNull(feeder);
        var cts = new CancellationTokenSource(10_000);
        // WHEN
        // await feeder.Activate(cts.Token);
        // if (feeder.Status == ComponentStatus.Active)
        while (!cts.Token.IsCancellationRequested)
        {
            feeder.HandleCast(StartFeeding.It(), cts.Token);
            await Task.Delay(100, cts.Token);
        }

        // if (feeder.Status == ComponentStatus.Active)
        feeder.HandleCast(StopFeeding.It(), cts.Token);
        // THEN
        // await feeder.Deactivate(cts.Token);
    }


    protected override void Initialize()
    {
        _executor = TestEnv.ResolveRequired<IHostExecutor>();
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddSettingsFromAppDirectory()
            .UseSettings<ESDBSettings>(ESDBSettings.SectionId)
            .AddConsoleLogger()
            .AddEventFeeder<TheSchema.DocID, TheSchema.Doc>()
            .AddTheDocCtors()
            .AddEventStreamGenFunc()
            .AddSingletonExchange()
            .AddResilientESDBClients()
            .AddSingletonESDBProjectorDriver<TheActors.IProjectorInfo>()
            .AddSingletonESDBProjector<TheActors.IProjectorInfo>()
            .AddESDBStore();
    }
}