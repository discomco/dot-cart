using DotCart.Core;

namespace DotCart.Contract.Dtos;

public interface IMsg
{
    string MsgId { get; }
    string Topic { get; }
    DateTime TimeStamp { get; }
    byte[] Data { get; }
    void SetTimeStamp(DateTime timeStamp);
    TPayload GetPayload<TPayload>() where TPayload : IPayload;
    void SetPayload<TPayload>(TPayload payload) where TPayload : IPayload;
    void SetData(byte[] data);
}

public abstract record Msg(string Topic, byte[] Data) : IMsg
{
    public string MsgId { get; } = GuidUtils.NewGuid;
    public string Topic { get; } = Topic;

    public byte[] Data { get; set; } = Data;

    public void SetData(byte[] data)
    {
        Data = data;
    }

    public TPayload GetPayload<TPayload>() where TPayload : IPayload
    {
        return Data == null
            ? default
            : Data.FromBytes<TPayload>();
    }

    public void SetPayload<TPayload>(TPayload payload) where TPayload : IPayload
    {
        if (payload == null) Data = Array.Empty<byte>();
        Data = payload.ToBytes();
    }

    public DateTime TimeStamp { get; private set; } = DateTime.UtcNow;

    public void SetTimeStamp(DateTime timeStamp)
    {
        TimeStamp = timeStamp;
    }
}

public abstract record Msg<TPayload>(string Topic, TPayload Payload)
    : Msg(Topic, Payload.ToBytes())
    where TPayload : IPayload
{
}