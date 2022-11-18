using DotCart.Context.Abstractions.Drivers;
using DotCart.Context.Drivers;
using DotCart.Contract.Schemas;
using Engine.Contract.Schema;

namespace Engine.Context.Common.Drivers;

[GroupName("engine-sub")]
[IDPrefix(Constants.EngineIDPrefix)]
public interface IEngineSubscriptionInfo : ISubscriptionInfo
{
}

// public class EngineESDBProjectorDriver : ESDBProjectorDriver<IEngineSubscriptionInfo>
// {
//     public EngineESDBProjectorDriver(
//         IESDBPersistentSubscriptionsClient client,
//         SubscriptionFilterOptions filterOptions) : base(TODO, client, filterOptions)
//     {
//     }
// }