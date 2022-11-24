using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.TestKit.Schema;

namespace DotCart.TestKit.Actors;

[GroupName(TestConstants.SubscriptionGroup)]
[IDPrefix(TestConstants.TheIDPrefix)]
public interface ITheSubscriptionInfo: ISubscriptionInfo
{
    
}