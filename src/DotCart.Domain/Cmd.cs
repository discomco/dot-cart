using DotCart.Schema;

namespace DotCart.Domain;

public interface ICmd
{
    string Topic { get; }
    IID GetID();
}

public interface ICmd<out TID, out TPayload> : ICmd
    where TID : IID<TID>
    where TPayload : IPayload
{
    TID AggregateID { get; }
    TPayload Payload { get; }
}

public abstract record Cmd<TID, TPayload>(
    string Topic,
    TID AggregateID,
    TPayload Payload
) : ICmd<TID, TPayload>
    where TID : IID<TID>
    where TPayload : IPayload
{
    public IID GetID()
    {
        return AggregateID;
    }
}