using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public delegate Command Hope2Cmd<TPayload, TMeta>(HopeT<TPayload> hope)
    where TPayload : IPayload
    where TMeta : IEventMeta;

// public record Command(
//     IID AggregateID,
//     TPayload Payload,
//     TMeta Meta) : ICommand
//     where TPayload : IPayload
//     where TMeta : IEventMeta
// {
//     public TMeta Meta { get; } = Meta;
//     public TPayload Payload { get; } = Payload;
//     public IID AggregateID { get; private set; } = AggregateID;
//
//     public string CmdType => CmdTopicAtt.Get<TPayload>();
//
//
//     public void SetID(IDB aggregateID)
//     {
//         AggregateID = aggregateID;
//     }
//
//     public static Command New(IID iD, TPayload payload, TMeta meta)
//     {
//         return new Command(iD, payload, meta);
//     }
// }