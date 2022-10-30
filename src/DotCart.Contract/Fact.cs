using DotCart.Schema;

namespace DotCart.Contract;

public interface IFact : IDto, IMsg
{
}

public abstract record Fact<TPayload>(string AggId, byte[] Data) : Dto(AggId, Data), IFact 
    where TPayload:IPayload
{
    public string MsgId { get; }
    public string MsgType { get; }
    public DateTime TimeStamp { get; private set; }

    public void SetTimeStamp(DateTime timeStamp)
    {
        TimeStamp = timeStamp;
    }

    public IPayload GetPayload()
    {
        return Data.FromBytes<TPayload>();
    }
}