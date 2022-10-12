using DotCart.Schema;

namespace DotCart.Domain;

public interface ICmd
{
    string Topic { get; }
    IID GetID();
}

public interface ICmd<out TPayload> : ICmd
    where TPayload : IPld
{
    IID AggregateID { get; }
    TPayload Pload { get; }
}

public abstract record Cmd<TPayload>(
    string Topic,
    IID AggregateID,
    TPayload Pload
) : ICmd<TPayload>
    where TPayload : IPld
{
    public IID GetID()
    {
        return AggregateID;
    }
    public TPayload Pload { get; }
}