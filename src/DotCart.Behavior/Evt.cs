using DotCart.Contract;
using DotCart.Schema;

namespace DotCart.Behavior;

public abstract record Evt<TPayload>(
    string MsgType,
    IID AggregateID,
    TPayload Payload) : Msg<TPayload>(MsgType, Payload), IEvt
    where TPayload : IPayload
{
    public string BehaviorType { get; private set; }
    public string AggregateId => AggregateID.Value;
    public long Version { get; private set; } = long.MaxValue;
    public byte[] MetaData { get; }
    public IID AggregateID { get; } = AggregateID;

    public void SetVersion(long version)
    {
        Version = version;
    }

    public void SetBehaviorType(string type)
    {
        BehaviorType = type;
    }
}

public interface IEvt : IMsg
{
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