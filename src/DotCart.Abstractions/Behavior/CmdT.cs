using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public abstract record CmdT<TPayload>(
    ID AggregateID,
    TPayload Payload
) : ICmd<TPayload>
    where TPayload : IPayload
{
    public void SetID(ID aggregateID)
    {
        AggregateID = aggregateID;
    }
    
    public TPayload Payload { get; } = Payload;
    public ID AggregateID { get; private set; } = AggregateID;
}