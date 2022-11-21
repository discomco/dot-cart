using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;

namespace Engine.Context.Common.Drivers;

[GroupName("engine-sub")]
[IDPrefix(Contract.Schema.EngineIDPrefix)]
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