using Ardalis.GuardClauses;
using DotCart.Drivers.Ardalis;

namespace DotCart.Abstractions.Drivers;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class GroupNameAttribute : Attribute
{
    public GroupNameAttribute(string groupName)
    {
        GroupName = groupName;
    }

    public string GroupName { get; }
}

public static class GroupNameAtt
{
    public static string Get<TInfo>() where TInfo : ISubscriptionInfoB
    {
        var atts = (GroupNameAttribute[])typeof(TInfo).GetCustomAttributes(typeof(GroupNameAttribute),
            true);
        Guard.Against.AttributeNotDefined("GroupName", atts, typeof(TInfo).Name);
        return atts[0].GroupName;
    }
}