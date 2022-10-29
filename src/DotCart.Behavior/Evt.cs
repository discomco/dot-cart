using DotCart.Schema;

namespace DotCart.Behavior;

public abstract record Evt<TPayload>(
    string EventType, 
    IID AggregateID, 
    TPayload Payload) : IEvt
    where TPayload: IPayload
{
    public string EventId { get; } = GuidUtils.NewGuid;
    
    public byte[] Data { get; }
    
    public DateTime TimeStamp { get; }
    
    public string AggregateType { get; }
    
    public string AggregateId => AggregateID.Value;

    public long Version { get; private set; }

    public byte[] MetaData { get; }

    public IPayload GetPayload()
    {
        return Payload;
    }
    public IID AggregateID { get; } = AggregateID;
    public void SetVersion(long version)
    {
        Version = version;
    }

    public TPayload Payload { get; } = Payload;
    public string EventType { get; } = EventType;
}


public interface IEvt
{
    string EventType { get; }
    string EventId { get; }
    byte[] Data { get; }
    DateTime TimeStamp { get; }
    string AggregateType { get; }
    string AggregateId { get; }
    long Version { get; }
    byte[] MetaData { get; }
    IPayload GetPayload();
    IID AggregateID { get; }

    void SetVersion(long version);
}


public interface IEvt<out TPayload> : IEvt where TPayload:IPayload
{
    TPayload Payload { get; }
}