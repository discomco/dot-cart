namespace DotCart.Abstractions.Behavior;

public delegate TMeta MetaCtorT<out TMeta>(string id);

public record MetaB(string AggregateName, string AggregateId, byte[]? Data = null) : IMetaB
{
    public static readonly byte[] Empty = Array.Empty<byte>();

    public string AggregateName { get; set; } = AggregateName;

    public string AggregateId { get; set; } = AggregateId;

    public byte[] Data { get; set; } = Data ?? Array.Empty<byte>();

    public void SetData(byte[] data)
    {
        Data = data;
    }

    public static MetaB New(string? fullName, string id, byte[]? data = null)
    {
        return new MetaB(fullName, id, data);
    }
}