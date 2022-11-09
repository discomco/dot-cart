using DotCart.Behavior;
using DotCart.Drivers.EventStoreDB.Interfaces;
using DotCart.Drivers.InMem;
using DotCart.Drivers.Serilog;
using DotCart.Effects.Drivers;
using DotCart.TestEnv.Engine;
using DotCart.TestEnv.Engine.Drivers;
using DotCart.TestEnv.Engine.Effects;
using DotCart.TestEnv.Engine.Schema;
using DotCart.TestKit;
using EventStore.Client;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit.Abstractions;

namespace DotCart.Drivers.EventStoreDB.Tests;

public class ESDBProjectorDriverTests : IoCTests
{
    private IProjectorDriver _driver;
    private IESDBPersistentSubscriptionsClient _client;
    private ILogger _logger;
    private SubscriptionFilterOptions _subOptions;
    private ESDBEngineEventFeeder _feeder;
    private IHostExecutor _executor;
    private IEventStoreDriver _eventStore;
    private EventStreamGenerator<EngineID,Engine> _newEventStream;

    public ESDBProjectorDriverTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }


    [Fact]
    public Task ShouldResolveProjectorDriver()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        _driver = Container.GetRequiredService<IProjectorDriver<IEngineSubscriptionInfo>>();
        // THEN
        Assert.NotNull(_driver);
        return Task.CompletedTask;
    }
    
    [Fact]
    public Task ShouldResolveSubscriptionFilterOptions()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        _subOptions = Container.GetRequiredService<SubscriptionFilterOptions>();
        // THEN
        Assert.NotNull(_subOptions);
        return Task.CompletedTask;
    }


    [Fact]
    public Task ShouldResolveHostExecutor()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        _executor = Container.GetRequiredService<IHostExecutor>();
        // THEN
        Assert.NotNull(_executor);
        return Task.CompletedTask;
    }

    [Fact]
    public async Task ShouldStartFeeder()
    {
        // GIVEN
        Assert.NotNull(_feeder);
        // WHEN
        var source = new CancellationTokenSource(10_000);
        
        var token = source.Token;

        await _executor.StartAsync(token);
        // THEN
        while (!token.IsCancellationRequested)
        { }

        await _executor.StopAsync(token);
        Assert.True(true);
    }

    [Fact]
    public void ShouldResolveEventStoreDriver()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        _eventStore = Container.GetRequiredService<IEventStoreDriver>();
        // THEN
        Assert.NotNull(_eventStore);


    }

    [Fact]
    public Task ShouldResolveLogger()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        _logger = Container.GetRequiredService<ILogger>();
        // THEN
        Assert.NotNull(_logger);
        return Task.CompletedTask;
    }
    

    [Fact]
    public void ShouldResolveESDBPersistentSubscriptionsClient()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        _client = Container.GetRequiredService<IESDBPersistentSubscriptionsClient>();
        // THEN
        Assert.NotNull(_client);
    }

    [Fact]
    public void ShouldResolveESDBEngineEventFeeder()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        _feeder = Container.GetHostedService<ESDBEngineEventFeeder>();
        // THEN 
        Assert.NotNull(_feeder);
    }

    [Fact]
    public void ShouldResolveEngineEventStreamGenerator()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        _newEventStream = Container.GetRequiredService<EventStreamGenerator<EngineID, Engine>>();
        // THEN
        Assert.NotNull(_newEventStream);
    }
    
    

    protected override void Initialize()
    {
        _executor = Container.GetRequiredService<IHostExecutor>();
        _feeder = Container.GetHostedService<ESDBEngineEventFeeder>();
        _newEventStream = Container.GetRequiredService<EventStreamGenerator<EngineID, Engine>>();
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
            .AddEngineESDBProjectorDriver()
            .AddConsoleLogger();

        if (TestKit.Config.IsCiCD)
        {
            services
                .AddMemEventStore()
                .AddSingleton(_ => A.Fake<IESDBPersistentSubscriptionsClient>());
        }
        else
        {
            services
                .AddConfiguredESDBClients()
                .AddESDBEventStoreDriver();

        }
    }
}