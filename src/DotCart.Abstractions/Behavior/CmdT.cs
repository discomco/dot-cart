using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public record CmdT<TPayload, TMeta>(
    IID AggregateID,
    TPayload Payload,
    TMeta Meta) : ICmdT<TPayload>
    where TPayload : IPayload
    where TMeta : IEventMeta
{
    public TMeta Meta { get; } = Meta;
    public TPayload Payload { get; } = Payload;
    public IID AggregateID { get; private set; } = AggregateID;

    public void SetID(IDB aggregateID)
    {
        AggregateID = aggregateID;
    }

    public static CmdT<TPayload, TMeta> New(IID iD, TPayload payload, TMeta meta) 
        => new(iD, payload, meta);
}