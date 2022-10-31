using DotCart.Schema;

namespace DotCart.Contract;

public interface IMsg
{
    string MsgId { get; }
    string MsgType { get; }
    DateTime TimeStamp { get; }
    void SetTimeStamp(DateTime timeStamp);
    IPayload GetPayload();
}

public abstract record Msg<TPayload>(string MsgType, TPayload Payload) : IMsg
    where TPayload : IPayload
{
    public string MsgId { get; } = GuidUtils.NewGuid;
    public string MsgType { get; } = MsgType;

    public IPayload GetPayload()
    {
        return Payload;
    }

    public DateTime TimeStamp { get; private set; } = DateTime.UtcNow;

    public void SetTimeStamp(DateTime timeStamp)
    {
        TimeStamp = timeStamp;
    }
}