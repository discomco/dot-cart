using DotCart.Context.Drivers;
using DotCart.Context.Effects;
using DotCart.Context.Effects.Drivers;
using DotCart.Contract.Schemas;
using DotCart.Drivers.EventStoreDB;
using DotCart.Drivers.EventStoreDB.Interfaces;
using Engine.Contract.Schema;
using EventStore.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context.Common.Drivers;

public static partial class Inject
{
    public static IServiceCollection AddEngineESDBProjectorDriver(this IServiceCollection services)
    {
        return services
            .AddProjector()
            .AddTransient(_ =>
                new SubscriptionFilterOptions(
                    StreamFilter.Prefix($"{IDPrefix.Get<IEngineSubscriptionInfo>()}{IDFuncs.PrefixSeparator}")))
            .AddTransient<IProjectorDriver, EngineESDBProjectorDriver>()
            .AddTransient<IProjectorDriver<IEngineSubscriptionInfo>, EngineESDBProjectorDriver>();
    }
}

[GroupName("engine-sub")]
[IDPrefix(Constants.EngineIDPrefix)]
public interface IEngineSubscriptionInfo : ISubscriptionInfo
{
}

public class EngineESDBProjectorDriver : ESDBProjectorDriver<IEngineSubscriptionInfo>
{
    public EngineESDBProjectorDriver(
        IESDBPersistentSubscriptionsClient client,
        SubscriptionFilterOptions filterOptions) : base(client, filterOptions)
    {
    }
}