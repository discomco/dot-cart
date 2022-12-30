using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Drivers;
using DotCart.Drivers.EventStoreDB.Interfaces;
using DotCart.Drivers.Mediator;
using DotCart.TestKit;
using EventStore.Client;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit.Abstractions;

namespace DotCart.Drivers.EventStoreDB.Tests;

public class ESDBProjectorTests : IoCTests
{
    private IESDBPersistentSubscriptionsClient? _client;
    private IProjectorDriver? _driver;
    private IEventStore? _eventStore;
    private IExchange? _exchange;
    private IHostExecutor? _executor;
    private ILogger? _logger;
    private IDocStore<TheSchema.Doc>? _memStore;
    private SubscriptionFilterOptions? _subOptions;

    public ESDBProjectorTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


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
        _client = TestEnv.ResolveRequired<IESDBPersistentSubscriptionsClient>();
        // THEN
        Assert.NotNull(_client);
    }


    protected override void Initialize()
    {
        _executor = TestEnv.ResolveRequired<IHostExecutor>();
    }

    protected override void SetEnVars()
    {
//        DotEnv.FromEmbedded();
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddSingletonExchange()
            .AddConfiguredESDBClients()
            .AddSingletonESDBProjectorDriver<TheActors.ISubscriptionInfo>()
            .AddESDBStore();
    }
}