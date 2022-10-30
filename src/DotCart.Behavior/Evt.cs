using DotCart.Schema;

namespace DotCart.Behavior;

public abstract record Evt<TPayload>(
    string EventType,
    IID AggregateID,
    TPayload Payload) : IEvt
    where TPayload : IPayload
{
    public string EventId { get; } = GuidUtils.NewGuid;


    public DateTime TimeStamp { get; }

    public string BehaviorType { get; private set; }

    public string AggregateId => AggregateID.Value;

    public long Version { get; private set; } = long.MaxValue;

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

    public void SetBehaviorType(string type)
    {
        BehaviorType = type;
    }

    public string EventType { get; } = EventType;
}

public interface IEvt
{
    string EventType { get; }
    string EventId { get; }
    DateTime TimeStamp { get; }
    string BehaviorType { get; }
    string AggregateId { get; }
    long Version { get; }
    byte[] MetaData { get; }
    IID AggregateID { get; }
    void SetVersion(long version);
    void SetBehaviorType(string type);

}

public interface IEvt<out TPayload> : IEvt where TPayload : IPayload
{
}