namespace DotCart.Behavior;

public record Event
{
    public string EventId { get; set; }
    string EventType { get; set; }
    byte[] Data { get; set; }
    DateTime Timestamp { get; set; }
    string AggregateType { get; set; }
    string AggregateId { get; set; }
    ulong Version { get; set; }
    byte[] MetaData { get; set; }
}

