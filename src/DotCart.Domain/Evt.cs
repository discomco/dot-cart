using DotCart.Schema;

namespace DotCart.Domain;

public abstract record Evt<TID, TPayload>(
    string Topic, 
    TID AggregateID, 
    TPayload Payload) : IEvt 
{
    public Guid EventId { get; } = Guid.NewGuid();
    public TID AggregateID { get; } = AggregateID;
    public TPayload Payload { get; } = Payload;
    public string Topic { get; } = Topic;
}


public interface IEvt
{
    string Topic { get; }
    Guid EventId { get; }
}


public interface IEvt<out TID, out TPayload> : IEvt where TID: IID<TID> where TPayload:IPayload
{
    TID AggregateID { get; }
    TPayload Payload { get; }
}