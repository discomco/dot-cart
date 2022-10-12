using DotCart.Schema;

namespace DotCart.Domain;

public abstract record Evt<TPayload>(
    string Topic, 
    IID AggregateID, 
    TPayload Payload) : IEvt
    where TPayload: IPld

{
    public Guid EventId { get; } = Guid.NewGuid();
    public IPld GetPayload()
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
    IPld GetPayload();
    IID AggregateID { get; }
}


public interface IEvt<out TPayload> : IEvt where TPayload:IPld
{
    TPayload Payload { get; }
}