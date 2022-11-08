using DotCart.Schema;

namespace DotCart.Behavior;

public record Event(ID AggregateID, string EventType, long Version, byte[] Data, byte[] MetaData, DateTime TimeStamp) : IEvt
{


    public static Event New<TPayload>(ID aggregateID, string eventType, TPayload payload, EventMeta meta, long version = Constants.NewAggregateVersion)
    {
        return new Event(
            aggregateID, 
            eventType, 
            version, 
            payload.ToBytes(), 
            meta.ToBytes(), 
            DateTime.UtcNow);
    }
    
    
    public long Version { get; set; } = Version;

    public string EventId { get; set; } = GuidUtils.LowerCaseGuid;

    public string EventType { get; set; } = EventType;

    public string MsgId => EventId;
    
    public string Topic => EventType;

    public DateTime TimeStamp { get; private set; } = TimeStamp;
    
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

    public byte[] Data { get; set; } = Data;
    
  
   
    public string AggregateId { get; set; }
    

    public void SetData(byte[] data)
    {
        Data = data;
    }

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
    
}

public record EventMeta(string AggregateType, string AggregateId)
{
    public string AggregateType { get; set; } = AggregateType;
    public string AggregateId { get; set; } = AggregateId;
    public static readonly byte[] Empty = Array.Empty<byte>();

    public static EventMeta New(string? fullName, string id)
    {
        return new EventMeta(fullName, id);
    }
}


