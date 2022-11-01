using DotCart.Schema;

namespace DotCart.Contract;

public record Dto(string AggId, byte[] Data) : IDto
{
    public string AggId { get; } = AggId;
    public byte[] Data { get; private set; } = Data;

    public T GetPayload<T>()
    {
        return !Data.Any()
            ? default
            : Data.FromBytes<T>();
    }

    public void SetPayload<T>(T state)
    {
        Data = state.ToBytes();
    }

    public string MsgId { get; }
    public string MsgType { get; }
    public DateTime TimeStamp { get; private set; }

    public void SetTimeStamp(DateTime timeStamp)
    {
        TimeStamp = timeStamp;
    }

    public IPayload GetPayload()
    {
        return GetPayload<IPayload>();
    }
}

public interface IDto : IMsg
{
    string AggId { get; }
    byte[] Data { get; }
    T GetPayload<T>();
    void SetPayload<T>(T state);
}