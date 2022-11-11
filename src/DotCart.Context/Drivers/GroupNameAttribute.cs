using Ardalis.GuardClauses;
using DotCart.Context.Effects.Drivers;
using DotCart.Drivers.Ardalis;

namespace DotCart.Context.Drivers;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class GroupNameAttribute : Attribute
{
    public GroupNameAttribute(string groupName)
    {
        GroupName = groupName;
    }

    public string GroupName { get; }
}

public static class GroupName
{
    public static string Get<TInfo>() where TInfo : ISubscriptionInfo
    {
        var atts = (GroupNameAttribute[])typeof(TInfo).GetCustomAttributes(typeof(GroupNameAttribute),
            true);
        Guard.Against.AttributeNotDefined("GroupName", atts, typeof(TInfo).Name);
        return atts[0].GroupName;
    }
}