using DotCart.Schema;

namespace DotCart.Contract;

public record Dto(string AggId, byte[] Data) : IDto
{
    public string AggId { get; } = AggId;
    public byte[] Data { get; private set; } = Data;
    public void SetData(byte[] data)
    {
        Data = data;
    }

    public T GetPayload<T>() where T : IPayload
    {
        return !Data.Any()
            ? default
            : Data.FromBytes<T>();
    }

    public void SetPayload<T>(T state) where T : IPayload
    {
        Data = state.ToBytes();
    }

    public string MsgId { get; }
    public string Topic { get; }
    public DateTime TimeStamp { get; private set; }

    public void SetTimeStamp(DateTime timeStamp)
    {
        TimeStamp = timeStamp;
    }
}

public interface IDto : IMsg
{
    string AggId { get; }

}