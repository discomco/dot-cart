using DotCart.Schema;

namespace DotCart.Contract;

public interface IMsg
{
    string MsgId { get; }
    string MsgType { get; }
    DateTime TimeStamp { get; }
    void SetTimeStamp(DateTime timeStamp);
    TPayload GetPayload<TPayload>();
    byte[] Data { get;  }
}

public abstract record Msg(string MsgType, byte[] Data) : IMsg
{
    public string MsgId { get; } = GuidUtils.NewGuid;
    public string MsgType { get; } = MsgType;

    public byte[] Data { get; set; } = Data;

    public TPayload GetPayload<TPayload>()
    {
        return Data == null
            ? default
            : Data.FromBytes<TPayload>();
    }
    public DateTime TimeStamp { get; private set; } = DateTime.UtcNow;

    public void SetTimeStamp(DateTime timeStamp)
    {
        TimeStamp = timeStamp;
    }
    
}

public abstract record Msg<TPayload>(string MsgType, TPayload Payload) 
    : Msg(MsgType, Payload.ToBytes())
    where TPayload : IPayload
{
}