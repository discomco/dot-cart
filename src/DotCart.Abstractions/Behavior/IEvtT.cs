using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

// public interface IEvtT<out TPayload, TMeta> : IEvtB
//     where TPayload : IPayload
//     where TMeta: IEventMeta
// {
// }

public interface IEvtT<out TPayload> : IEvtB
    where TPayload : IPayload
{
}

public interface IEventMeta
{
}

public interface IEventMeta<TAggregate> : IEventMeta
{
}