namespace DotCart.Abstractions.Behavior;

public delegate TMeta MetaCtorT<out TMeta>(string id);

public record Meta(string AggregateName, string AggregateId) : IMeta
{
    public static readonly byte[] Empty = Array.Empty<byte>();

    public string AggregateName { get; set; } = AggregateName;

    public string AggregateId { get; set; } = AggregateId;

    public byte[]? Data { get; set; } = Array.Empty<byte>();

    public static Meta New(string? fullName, string id, byte[]? data = null)
    {
        data ??= Array.Empty<byte>();
        return new Meta(fullName, id);
    }
}