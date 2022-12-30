using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;

namespace DotCart.TestKit;

public static class TheActors
{
    [GroupName(TheConstants.SubscriptionGroup)]
    [IDPrefix(TheConstants.IDPrefix)]
    public interface ISubscriptionInfo : ISubscriptionInfoB
    {
    }
}