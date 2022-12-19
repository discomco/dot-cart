using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using Engine.Contract;

namespace Engine.Context;

[GroupName("engine-sub")]
[IDPrefix(Schema.EngineIDPrefix)]
public interface IEngineSubscriptionInfo : ISubscriptionInfo
{
}