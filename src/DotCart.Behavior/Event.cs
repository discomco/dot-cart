using DotCart.Schema;

namespace DotCart.Behavior;

public record Event(string EventType, long Version, byte[] Data, byte[] MetaData, DateTime TimeStamp) : IEvt
{
    public long Version { get; set; } = Version;

    public string EventId { get; set; }

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

    public SimpleID AggregateID { get; }
    
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

public record EventMeta
{
    public string AggregateType { get; set; }
}


