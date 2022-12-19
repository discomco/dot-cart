namespace DotCart.Abstractions.Behavior;

public delegate EventMeta MetaCtor(string id);

public record EventMeta(string AggregateType, string AggregateId) : IEventMeta
{
    public static readonly byte[] Empty = Array.Empty<byte>();

    public string AggregateType { get; set; } = AggregateType;

    public string AggregateId { get; set; } = AggregateId;

    public static EventMeta New(string? fullName, string id)
    {
        return new EventMeta(fullName, id);
    }
}