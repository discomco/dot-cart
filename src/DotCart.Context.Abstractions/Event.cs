using DotCart.Contract.Dtos;
using DotCart.Contract.Schemas;
using DotCart.Core;

namespace DotCart.Context.Abstractions;

public static class Constants
{
    public const long NewAggregateVersion = -1;
}

public record Event(ID AggregateID, string EventType, long Version, byte[] Data, byte[] MetaData,
    DateTime TimeStamp) : IEvt
{
    public string EventId { get; set; } = GuidUtils.LowerCaseGuid;

    public string EventType { get; set; } = EventType;


    public string AggregateId { get; set; }

    public string MsgId => EventId;

    public string Topic => EventType;

    public DateTime TimeStamp { get; private set; } = TimeStamp;

    public byte[] Data { get; set; } = Data;


    public long Version { get; set; } = Version;

    public void SetVersion(long version)
    {
        Version = version;
    }

    public ID AggregateID { get; } = AggregateID;

    public byte[] MetaData { get; set; } = MetaData;

    public void SetMetaPayload<TPayload>(TPayload payload)
    {
        MetaData = payload == null
            ? Array.Empty<byte>()
            : payload.ToBytes();
    }

    public TPayload GetMetaPayload<TPayload>()
    {
        return MetaData == null
            ? default
            : MetaData.FromBytes<TPayload>();
    }

    public void SetTimeStamp(DateTime timeStamp)
    {
        TimeStamp = timeStamp;
    }

    public TPayload GetPayload<TPayload>() where TPayload : IPayload
    {
        return Data.FromBytes<TPayload>();
    }

    public void SetPayload<TPayload>(TPayload payload) where TPayload : IPayload
    {
        Data = payload.ToBytes();
    }


    public void SetData(byte[] data)
    {
        Data = data;
    }

    public static Event New<TPayload>(ID aggregateID,
        string eventType,
        TPayload payload,
        EventMeta meta,
        long version = Constants.NewAggregateVersion)
    {
        return new Event(
            aggregateID,
            eventType,
            version,
            payload.ToBytes(),
            meta.ToBytes(),
            DateTime.UtcNow);
    }

    public static Event New(ID aggregateID,
        string eventType,
        byte[] data,
        byte[] meta,
        long version,
        DateTime timeStamp)
    {
        return new Event(
            aggregateID,
            eventType,
            version,
            data,
            meta,
            timeStamp);
    }
}

public record EventMeta(string AggregateType, string AggregateId)
{
    public static readonly byte[] Empty = Array.Empty<byte>();
    public string AggregateType { get; set; } = AggregateType;
    public string AggregateId { get; set; } = AggregateId;

    public static EventMeta New(string? fullName, string id)
    {
        return new EventMeta(fullName, id);
    }
}