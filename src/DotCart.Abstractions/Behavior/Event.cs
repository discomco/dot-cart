using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.Abstractions.Behavior;

public static class EventConst
{
    public const long NewAggregateVersion = -1;
}

public record Event(
    string AggregateId,
    string EventType,
    byte[] Data,
    byte[] MetaData) : IEvtB
{
    public IID AggregateID => IDB.New(AggregateId.PrefixFromIdString(), AggregateId.ValueFromIdString());
    public string EventType { get; set; } = EventType;

    public string AggregateId { get; set; } = AggregateId;
    public string EventId { get; set; } = GuidUtils.LowerCaseGuid;
    public string Topic => EventType;
    public DateTime TimeStamp { get; private set; }

    public byte[] Data { get; set; } = Data;
    public bool IsCommitted { get; private set; }

    public long Version { get; private set; }

    public void SetIsCommitted(bool isCommitted)
    {
        IsCommitted = isCommitted;
    }

    public void SetVersion(long version)
    {
        Version = version;
    }

    public byte[] MetaData { get; set; } = MetaData;

    public void SetMeta<TMeta>(TMeta meta) where TMeta : IEventMeta
    {
        MetaData = meta == null
            ? Array.Empty<byte>()
            : meta.ToBytes();
    }

    public TMeta GetMeta<TMeta>() where TMeta : IEventMeta
    {
        return MetaData == null
            ? default
            : MetaData.FromBytes<TMeta>();
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

    public void SetEventType(string eventType)
    {
        EventType = eventType;
    }

    public void SetMetaData(byte[] metaData)
    {
        MetaData = metaData;
    }

    // public static Event New<TPayload>(
    //     IID aggregateID,
    //     string eventType,
    //     TPayload payload,
    //     EventMeta meta,
    //     long version = Constants.NewAggregateVersion)
    // {
    //     return new Event(
    //         aggregateID,
    //         eventType,
    //         version,
    //         payload.ToBytes(),
    //         meta.ToBytes(),
    //         DateTime.UtcNow);
    // }

    public static Event New(
        IID aggregateID,
        string eventType,
        byte[] data,
        byte[] meta)
    {
        return new Event(
            aggregateID.Id(),
            eventType,
            data,
            meta);
    }
}