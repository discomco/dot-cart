using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using Xunit;

namespace DotCart.TestFirst.Actors;

public abstract class ProjectionInfoTestsT<TInfo>
    where TInfo : IProjectorInfoB
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