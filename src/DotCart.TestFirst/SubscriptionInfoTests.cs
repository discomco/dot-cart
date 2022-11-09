using DotCart.Drivers;
using DotCart.Effects.Drivers;
using DotCart.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst;

public abstract class SubscriptionInfoTests<TInfo>: IoCTests where TInfo : ISubscriptionInfo
{

    [Fact]
    public void ShouldHaveGroupName()
    {
        // GIVEN
        var groupName = string.Empty;
        // WHEN
        groupName = GroupName.Get<TInfo>();
        // THEN
        Assert.NotEmpty(groupName);
    }

    [Fact]
    public void ShouldHaveIDPrefix()
    {
        // GIVEN
        var prefix = string.Empty;
        // WHEN
        prefix = IDPrefix.Get<TInfo>();
        // THEN
        Assert.NotEmpty(prefix);
    }


    protected SubscriptionInfoTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }
}