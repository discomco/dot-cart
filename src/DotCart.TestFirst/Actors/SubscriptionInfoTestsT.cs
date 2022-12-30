using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;

namespace DotCart.TestFirst.Actors;

public abstract class SubscriptionInfoTestsT<TInfo> where TInfo : ISubscriptionInfoB
{
    [Fact]
    public void ShouldHaveGroupName()
    {
        // GIVEN
        var groupName = string.Empty;
        // WHEN
        groupName = GroupNameAtt.Get<TInfo>();
        // THEN
        Assert.NotEmpty(groupName);
    }

    [Fact]
    public void ShouldHaveIDPrefix()
    {
        // GIVEN
        var prefix = string.Empty;
        // WHEN
        prefix = IDPrefixAtt.Get<TInfo>();
        // THEN
        Assert.NotEmpty(prefix);
    }
}