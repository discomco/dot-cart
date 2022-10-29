using DotCart.Schema;

namespace DotCart.Behavior;

public interface ICmd
{
    string Topic { get; }
    IID GetID();
}

public interface ICmd<out TPayload> : ICmd
    where TPayload : IPayload
{
    IID AggregateID { get; }
    TPayload Payload { get; }
}

public abstract record Cmd<TPayload>(
    string Topic,
    IID AggregateID,
    TPayload Payload
) : ICmd<TPayload>
    where TPayload : IPayload
{
    public IID GetID()
    {
        return AggregateID;
    }

    public TPayload Payload { get; } = Payload;
}