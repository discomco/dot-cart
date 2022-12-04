using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public interface IEvtT<out TPayload, TMeta> : IEvt
    where TPayload : IPayload
    where TMeta: IEventMeta
{
}

public interface IEventMeta
{
}

public interface IEventMeta<TAggregate> : IEventMeta {}