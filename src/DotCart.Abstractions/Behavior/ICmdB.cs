using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public interface ICmdB
{
    IID AggregateID { get; }
}

public interface ICmdT<out TPayload> : ICmdB
    where TPayload : IPayload

{
    TPayload Payload { get; }
}