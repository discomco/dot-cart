using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public abstract record CmdT<TPayload>(
    string Topic,
    IID AggregateID,
    TPayload Payload
) : ICmd<TPayload>
    where TPayload : IPayload
{
    public TPayload Payload { get; } = Payload;
    public IID AggregateID { get; } = AggregateID;
}