using DotCart.Context.Drivers;
using DotCart.Context.Effects.Drivers;
using DotCart.Contract.Schemas;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst;

public abstract class SubscriptionInfoTests<TInfo> : IoCTests where TInfo : ISubscriptionInfo
{
    protected SubscriptionInfoTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

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
}