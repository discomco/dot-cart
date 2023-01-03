using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.Abstractions.Behavior;

public delegate CmdT<TPayload, TMeta> Hope2Cmd<TPayload, TMeta>(HopeT<TPayload> hope)
    where TPayload : IPayload
    where TMeta : IEventMeta;

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
        return new CmdT<TPayload, TMeta>(iD, payload, meta);
    }
}