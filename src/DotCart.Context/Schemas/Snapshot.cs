namespace DotCart.Context.Schemas;

public record Snapshot(string Id, string BehaviorType, byte[] Data, long Version)
{
    public string Id { get; } = Id;
    public string BehaviorType { get; } = BehaviorType;
    public byte[] Data { get; } = Data;
    public long Version { get; } = Version;
}