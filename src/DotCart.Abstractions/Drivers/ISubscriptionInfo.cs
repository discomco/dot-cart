namespace DotCart.Abstractions.Drivers;

public interface ISubscriptionInfo
{
    string GroupName { get; set; }
    string Prefix { get; set; }
}

public record SubscriptionInfo(string GroupName, string Prefix) : ISubscriptionInfo
{
    public string GroupName { get; set; } = GroupName;
    public string Prefix { get; set; } = Prefix;
}