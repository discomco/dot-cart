using DotCart.Drivers;
using DotCart.Drivers.EventStoreDB;
using DotCart.Drivers.EventStoreDB.Interfaces;
using DotCart.Effects.Drivers;
using DotCart.Schema;
using DotCart.TestEnv.Engine.Schema;
using EventStore.Client;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DotCart.TestEnv.Engine.Drivers;


public static class Inject
{
    public static IServiceCollection AddEngineESDBProjectorDriver(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => 
                new SubscriptionFilterOptions(
                    StreamFilter.Prefix(IDPrefix.Get<IEngineSubscriptionInfo>())))
            .AddTransient<IProjectorDriver<IEngineSubscriptionInfo>,EngineESDBProjectorDriver>();

    }
}


[GroupName("engine-sub")]
[IDPrefix(Constants.EngineIDPrefix)]
public interface IEngineSubscriptionInfo: ISubscriptionInfo {}

public class EngineESDBProjectorDriver: ESDBProjectorDriver<IEngineSubscriptionInfo>
    , IProjectorDriver<IEngineSubscriptionInfo>
{
    public EngineESDBProjectorDriver(
        ILogger logger,
        IESDBPersistentSubscriptionsClient client,
        SubscriptionFilterOptions filterOptions) : base(logger, client, filterOptions)
    {}
}