using DotCart.Behavior;
using DotCart.Drivers.EventStoreDB.Interfaces;
using DotCart.Drivers.Serilog;
using DotCart.Effects.Drivers;
using DotCart.TestEnv.Engine.Drivers;
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

    public ESDBProjectorDriverTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }


    [Fact]
    public void ShouldResolveProjectorDriver()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        _driver = Container.GetRequiredService<IProjectorDriver<IEngineSubscriptionInfo>>();
        // THEN
        Assert.NotNull(_driver);
    }
    
    [Fact]
    public void ShouldResolveSubscriptionFilterOptions()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        _subOptions = Container.GetRequiredService<SubscriptionFilterOptions>();
        // THEN
        Assert.NotNull(_subOptions);
    }

    [Fact]
    public void ShouldResolveLogger()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        _logger = Container.GetRequiredService<ILogger>();
        // THEN
        Assert.NotNull(_logger);
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

    protected override void Initialize()
    {
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddEngineESDBProjectorDriver()
            .AddSingleton(_ => A.Fake<IESDBPersistentSubscriptionsClient>())
            .AddConsoleLogger();
        
        // TODO:
//            .AddConfiguredESDBClients()
 //           .AddESDBProjectorDriver();
    }
}