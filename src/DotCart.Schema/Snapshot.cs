namespace DotCart.Schema;


public class Snapshot
{
    public string Id { get; set; }
    public string BehaviorType { get; set; }
    public byte[] Data { get; set; }
    public long Version { get; set; }
}