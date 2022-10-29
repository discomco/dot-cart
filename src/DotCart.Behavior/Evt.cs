using DotCart.Schema;

namespace DotCart.Behavior;

public abstract record Evt<TPayload>(
    string Topic, 
    IID AggregateID, 
    TPayload Payload) : IEvt
    where TPayload: IPayload

{
    public Guid EventId { get; } = Guid.NewGuid();
    public IPayload GetPayload()
    {
        return Payload;
    }
    public IID AggregateID { get; } = AggregateID;
    public TPayload Payload { get; } = Payload;
    public string Topic { get; } = Topic;
}


public interface IEvt
{
    string Topic { get; }
    Guid EventId { get; }
    IPayload GetPayload();
    IID AggregateID { get; }
}


public interface IEvt<out TPayload> : IEvt where TPayload:IPayload
{
    TPayload Payload { get; }
}