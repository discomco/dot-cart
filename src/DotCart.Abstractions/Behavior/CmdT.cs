using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.Abstractions.Behavior;

public record CmdT<TPayload, TMeta>(
    IID AggregateID,
    TPayload Payload,
    TMeta Meta) : ICmdT<TPayload, TMeta>
    where TPayload : IPayload
    where TMeta : IEventMeta
{
    public TMeta Meta { get; } = Meta;
    public TPayload Payload { get; } = Payload;
    public IID AggregateID { get; private set; } = AggregateID;

    public string CmdType => CmdTopicAtt.Get<TPayload>();


    public void SetID(IDB aggregateID)
    {
        AggregateID = aggregateID;
    }

    public static CmdT<TPayload, TMeta> New(IID iD, TPayload payload, TMeta meta)
    {
        return new(iD, payload, meta);
    }
}