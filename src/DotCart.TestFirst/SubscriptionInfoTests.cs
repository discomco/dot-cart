using DotCart.Effects.Drivers;
using DotCart.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.Drivers.EventStoreDB.Tests;

public abstract class SubscriptionInfoTests<TSubscriptionInfo>: IoCTests where TSubscriptionInfo : ISubscriptionInfo
{

    [Fact]
    public void ShouldHaveGroupName()
    {
        // GIVEN
        var groupName = string.Empty;
        // WHEN
        groupName = GroupName.Get<TSubscriptionInfo>();
        // THEN
        Assert.NotEmpty(groupName);
    }

    [Fact]
    public void ShouldHaveIDPrefix()
    {
        // GIVEN
        var prefix = string.Empty;
        // WHEN
        prefix = IDPrefix.Get<TSubscriptionInfo>();
        // THEN
        Assert.NotEmpty(prefix);
    }


    protected SubscriptionInfoTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }
}