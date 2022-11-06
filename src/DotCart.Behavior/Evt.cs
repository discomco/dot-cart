using DotCart.Contract;
using DotCart.Schema;


namespace DotCart.Behavior;

public interface IEvt : IMsg
{
    byte[] MetaData { get; set; }
    string AggregateId { get; set; }
    long Version { get; set; }
}

public interface IEvt<out TPayload> : IEvt 
    where TPayload : IPayload 
{
}


public abstract record Evt<TPayload>(string MsgType, IID AggregateID, TPayload Payload)
    : Msg<TPayload>(MsgType, Payload), IEvt
    where TPayload : IPayload
{

    public byte[] MetaData { get; set; }
    public string AggregateId { get; set; } = AggregateID.Value;
    public long Version { get; set; } = long.MaxValue;
    public Guid EventID => Guid.Parse(MsgId);
}

