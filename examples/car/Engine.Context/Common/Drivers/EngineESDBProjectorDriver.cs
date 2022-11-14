using DotCart.Context.Abstractions.Drivers;
using DotCart.Context.Drivers;
using DotCart.Contract.Schemas;
using DotCart.Drivers.EventStoreDB;
using DotCart.Drivers.EventStoreDB.Interfaces;
using Engine.Contract.Schema;
using EventStore.Client;

namespace Engine.Context.Common.Drivers;

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