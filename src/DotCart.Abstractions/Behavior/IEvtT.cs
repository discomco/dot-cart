using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public interface IEvtT<out TPayload> : IEvt
    where TPayload : IPayload
{
}