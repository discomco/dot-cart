using DotCart.Client.Contracts;
using DotCart.Client.Schemas;
using DotCart.Core;

namespace DotCart.Context.Behaviors;

public interface IEvt : IMsg
{
    ID AggregateID { get; }
    byte[] MetaData { get; set; }
    long Version { get; set; }
    void SetVersion(long version);
    void SetMetaPayload<TPayload>(TPayload payload);
    TPayload GetMetaPayload<TPayload>();
}

public interface IEvt<out TPayload> : IEvt
    where TPayload : IPayload
{
}

public abstract record Evt<TPayload>(string Topic, ID AggregateID, TPayload Payload)
    : Msg<TPayload>(Topic, Payload), IEvt
    where TPayload : IPayload
{
    public Guid EventID => Guid.Parse(MsgId);

    public ID AggregateID { get; set; } = AggregateID;
    public byte[] MetaData { get; set; }
    public long Version { get; set; } = long.MaxValue;

    public void SetData(byte[] data)
    {
        Data = data;
    }

    public void SetVersion(long version)
    {
        Version = version;
    }

    public void SetMetaPayload<T>(T payload)
    {
        MetaData = payload == null ? Array.Empty<byte>() : payload.ToBytes();
    }

    public T GetMetaPayload<T>()
    {
        return MetaData.FromBytes<T>();
    }
}