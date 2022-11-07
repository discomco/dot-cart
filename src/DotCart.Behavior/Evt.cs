using DotCart.Contract;
using DotCart.Schema;


namespace DotCart.Behavior;

public interface IEvt : IMsg
{
    byte[] MetaData { get; set; }
    string AggregateId { get; set; }
    long Version { get; set; }
    void SetData(byte[] data);
    void SetVersion(long version);
}

public interface IEvt<out TPayload> : IEvt 
    where TPayload : IPayload 
{
}


public abstract record Evt<TID,TPayload>(string MsgType, TID AggregateID, TPayload Payload)
    : Msg<TPayload>(MsgType, Payload), IEvt
    where TPayload : IPayload
    where TID: IID<TID>
{

    public TID AggregateID { get; set; } = AggregateID;
    public byte[] MetaData { get; set; }
    public string AggregateId { get; set; } 
    public long Version { get; set; } = long.MaxValue;
    public void SetData(byte[] data)
    {
        Data = data;
    }

    public void SetVersion(long version)
    {
        Version = version;
    }

    public Guid EventID => Guid.Parse(MsgId);
}

